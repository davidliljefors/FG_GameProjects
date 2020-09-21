// Fill out your copyright notice in the Description page of Project Settings.

#include "GravityCharacter.h"
#include "GravityMovementComponent.h"
#include "Camera/CameraComponent.h"
#include "Components/CapsuleComponent.h"
#include "Components/BoxComponent.h"
#include "Components/InputComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "Kismet/KismetMathLibrary.h"
#include "Kismet/KismetSystemLibrary.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "GravitySwappable.h"
#include "ClickInteract.h"	
#include "ClickInteractComponent.h"
#include "DrawDebugHelpers.h"
#include "GravitySwapComponent.h"
#include "TimerManager.h"

// Sets default values
AGravityCharacter::AGravityCharacter(const FObjectInitializer& ObjectInitializer)
	: Super(ObjectInitializer.SetDefaultSubobjectClass<UGravityMovementComponent>(ACharacter::CharacterMovementComponentName))
{
	PrimaryActorTick.bCanEverTick = true;
	CachedGravityMovementyCmp = Cast<UGravityMovementComponent>(GetMovementComponent());

	// Set size for collision capsule
	GetCapsuleComponent()->InitCapsuleSize(42.f, 96.0f);

	// Don't rotate when the controller rotates.
	bUseControllerRotationPitch = false;
	bUseControllerRotationYaw = false;
	bUseControllerRotationRoll = false;

	// Create a camera boom attached to the root (capsule)
	CameraBoom = CreateDefaultSubobject<USpringArmComponent>(TEXT("CameraBoom"));
	CameraBoom->SetupAttachment(RootComponent);
	CameraBoom->SetUsingAbsoluteRotation(true); // Rotation of the character should not affect rotation of boom
	CameraBoom->bDoCollisionTest = false;
	CameraBoom->TargetArmLength = 500.f;
	CameraBoom->SocketOffset = FVector(0.f, 0.f, 75.f);
	CameraBoom->SetRelativeRotation(FRotator(0.f, 180.f, 0.f));

	// Create a camera and attach to boom
	SideViewCameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("SideViewCamera"));
	SideViewCameraComponent->SetupAttachment(CameraBoom, USpringArmComponent::SocketName);
	SideViewCameraComponent->bUsePawnControlRotation = false; // We don't want the controller rotating the camera

	// Configure character movement
	GetCharacterMovement()->bOrientRotationToMovement = true; // Face in the direction we are moving..
	GetCharacterMovement()->RotationRate = FRotator(0.0f, 720.0f, 0.0f); // ...at this rotation rate
	GetCharacterMovement()->GravityScale = 2.f;
	GetCharacterMovement()->AirControl = 0.80f;
	GetCharacterMovement()->JumpZVelocity = 1000.f;
	GetCharacterMovement()->GroundFriction = 3.f;
	GetCharacterMovement()->MaxWalkSpeed = 600.f;
	GetCharacterMovement()->MaxFlySpeed = 600.f;

	// Note: The skeletal mesh and anim blueprint references on the Mesh component (inherited from Character) 
	// are set in the derived blueprint asset named MyCharacter (to avoid direct content references in C++)

	// Approach Interaction Box Setup
	InteractBox = CreateDefaultSubobject<UBoxComponent>(TEXT("Interaction box"));
	InteractBox->SetupAttachment(RootComponent);
	InteractBox->OnComponentBeginOverlap.AddDynamic(this, &AGravityCharacter::OnInteractBoxBeginOverlap);
	InteractBox->OnComponentEndOverlap.AddDynamic(this, &AGravityCharacter::OnInteractBoxEndOverlap);

	// ClickBox
	ClickBox = CreateDefaultSubobject<UBoxComponent>(TEXT("ClickBox"));
	ClickBox->SetupAttachment(RootComponent);
}

void AGravityCharacter::BeginPlay()
{
	Super::BeginPlay();

	BaseWalkSpeed = CachedGravityMovementyCmp->MaxWalkSpeed;

	// UpdateOutlineUnderCursor timer
	FTimerHandle OutlineUpdateTimer;
	FTimerDelegate OutlineDelegate = FTimerDelegate::CreateLambda([=]() { this->UpdateOutlineUnderCursor(); });
	GetWorldTimerManager().SetTimer(OutlineUpdateTimer, OutlineDelegate, 0.5f, true);
}

void AGravityCharacter::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	// Keep Character locked in X axis at all times
	FVector ConstrainedLocation = GetActorLocation();
	ConstrainedLocation.X = 0.0f;
	SetActorLocation(ConstrainedLocation);

	// Apply gravity to character
	const FVector OldGravityDir = CachedGravityMovementyCmp->GetGravityDirection();
	FVector NewGravityDir = GravityPoint - GetActorLocation();
	NewGravityDir.Normalize();
	if (bFlipGravity)
	{
		NewGravityDir = NewGravityDir * -1.f;
	}

	//NewGravityDir = FMath::VInterpTo(OldGravityDir, NewGravityDir, DeltaTime, GravityChangeSpeed);
	CachedGravityMovementyCmp->SetGravityDirection(NewGravityDir);

	// Calculate three vector to make the rotation space for camera
	const FVector ForwardVector{ -1.0f, 0.0f, 0.0f };
	const FVector UpVector = UKismetMathLibrary::GetDirectionUnitVector(GravityPoint, GetActorLocation());
	const FVector RightVector = FVector::CrossProduct(UpVector, ForwardVector);

	// Calculate the rotation and set rotation
	FRotator TargetRotation = UKismetMathLibrary::MakeRotationFromAxes(ForwardVector, RightVector, UpVector);
	CameraBoom->SetWorldRotation(TargetRotation);

	// Release box if we get too far away
	if (ApproachInteractableComp && IsGrabbing())
	{
		auto DistSqrBoxToPlayer = FVector::DistSquared(GetActorLocation(), ApproachInteractableComp->GetOwner()->GetActorLocation());
		UE_LOG(LogTemp, Warning, L"Distance = %f", DistSqrBoxToPlayer);
		if (DistSqrBoxToPlayer > AutoReleaseBoxDistance * AutoReleaseBoxDistance)
		{
			OnApproachInteractReleased();
		}
	}
}

void AGravityCharacter::MoveRight(float Val)
{
	if (Val == 0) { return; }

	// Add movement with consideration to the direction of camera
	FVector LocalMove{ 0.0f, 1.0f, 0.0f };
	FQuat CameraRotation = CameraBoom->GetComponentRotation().Quaternion();
	FVector TranslatedMove = CameraRotation * LocalMove;
	TranslatedMove.X = 0.0f;
	AddMovementInput(TranslatedMove, Val);

	// if player has CurrentClickFocus then reset all click components within range when player moves
	OnResetClickAction();
	// Empty the cached clickables
	ClickCompListToReset.Empty();
}

void AGravityCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	// set up gameplay key bindings
	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &AGravityCharacter::Jump);
	PlayerInputComponent->BindAction("Jump", IE_Released, this, &ACharacter::StopJumping);
	PlayerInputComponent->BindAxis("MoveRight", this, &AGravityCharacter::MoveRight);
	PlayerInputComponent->BindAction("Interact", IE_Pressed, this, &AGravityCharacter::OnApproachInteract);
	//PlayerInputComponent->BindAction("Interact", IE_Released, this, &AGravityCharacter::OnInteractReleased);
	//PlayerInputComponent->BindAction("LeftMouseButton", IE_Released, this, &AGravityCharacter::OnClick);
}

bool AGravityCharacter::GetFlipGravity() const
{
	return bFlipGravity;
}

void AGravityCharacter::SetFlipGravity(bool bNewGravity)
{
	bFlipGravity = bNewGravity;
}

void AGravityCharacter::AddCollectible(ACollectible* Collectible)
{
	if (Collectible != nullptr)
	{
		if (Collectible->GetCount() <= 0)
		{
			UE_LOG(LogTemp, Error, TEXT("Collitible has invalid count"));
		}

		if (Collectibles.Contains(Collectible->GetType()))
		{
			Collectibles[Collectible->GetType()] += Collectible->GetCount();
		}
		else
		{
			Collectibles.Add(Collectible->GetType(), Collectible->GetCount());
		}

		// Notify blueprint 
		OnCollectibleAdded(Collectible->GetType(), Collectibles[Collectible->GetType()]);
	}
	else
	{
		UE_LOG(LogTemp, Error, TEXT("Collitible was nullptrs"));
	}
}

void AGravityCharacter::SetGravityTarget(FVector NewGravityPoint)
{
	GravityPoint = NewGravityPoint;
}

#pragma region Jump

void AGravityCharacter::Jump()
{
	if (IsGrabbing())
	{
		OnApproachInteractReleased();
	}

	OnResetClickAction();

	ACharacter::Jump();
}

bool AGravityCharacter::IsJumping()
{
	return CachedGravityMovementyCmp->IsFalling();
}

#pragma endregion

#pragma region Approach Interaction

//////////////////////////////////////
/// Approach Interact
void AGravityCharacter::OnInteractBoxBeginOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	ApproachInteractableComp = TryGetApproachInteractableComp();
	if (ApproachInteractableComp == nullptr) { return; }

	ApproachInteractableComp->ShowInteractionWidget();
	UE_LOG(LogTemp, Warning, TEXT("Closest Overlap Actor: %s"), *ApproachInteractableComp->GetOwner()->GetName());
}

void AGravityCharacter::OnInteractBoxEndOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex)
{
	if (ApproachInteractableComp == nullptr) { return; }

	ApproachInteractableComp->HideInteractionWidget();
}

void AGravityCharacter::OnApproachInteract()
{
	// return if the player is currently jumping or grabbing something
	if (IsJumping() || IsGrabbing() || !CachedGravityMovementyCmp->IsMovingOnGround())
	{
		UE_LOG(LogTemp, Warning, TEXT("Cannot ApproachInteract while jumping nor grabbing. Calling OnInteractReleased"));
		OnApproachInteractReleased();
		return;
	}

	OnResetClickAction();

	ApproachInteractableComp = TryGetApproachInteractableComp();

	// Try to get a ApproachInteractableComponent and execute its Interact() method
	// ApproachInteractableComp = TryGetApproachInteractableComp();
	if (ApproachInteractableComp != nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Interacts with : %s"), *ApproachInteractableComp->GetName());
		CachedGravityMovementyCmp->MaxWalkSpeed = PushSpeed;
		IApproachInteract::Execute_Interact(ApproachInteractableComp);
	}
}

void AGravityCharacter::OnApproachInteractReleased()
{
	CachedGravityMovementyCmp->MaxWalkSpeed = BaseWalkSpeed;

	if (ApproachInteractableComp != nullptr)
	{
		IApproachInteract::Execute_InteractReleased(ApproachInteractableComp);
	}
	ApproachInteractableComp = nullptr;
}

UApproachInteractComponent* AGravityCharacter::TryGetApproachInteractableComp()
{
	TArray<AActor*> OverlappingActors;
	InteractBox->GetOverlappingActors(OverlappingActors);
	if (OverlappingActors.Num() == 0)
	{
		UE_LOG(LogTemp, Warning, TEXT("OverlappingActors : 0"));
		return nullptr;
	}

	// Get the closest actor among the Overlapping actors
	AActor* ClosestActor = OverlappingActors[0];
	for (auto CurrentActor : OverlappingActors)
	{
		if (GetDistanceTo(CurrentActor) < GetDistanceTo(ClosestActor))
		{
			ClosestActor = CurrentActor;
		}
	}

	return Cast<UApproachInteractComponent>(GetComponentByInterface<UApproachInteract>(ClosestActor));
}
/// Approach Interact
//////////////////////////////////////

#pragma endregion Approach Interaction

void AGravityCharacter::ResetWalkSpeed()
{
	CachedGravityMovementyCmp->MaxWalkSpeed = BaseWalkSpeed;
}

#pragma region Click & Swap Interaction

void AGravityCharacter::OnClick()
{
	//if (TryGetClickCompUnderCursor() == nullptr)
	//{
	//	UE_LOG(LogTemp, Warning, TEXT("Hit Nothing"));
	//	OnResetClickAction();
	//}
}

void AGravityCharacter::OnResetClickAction()
{
	if (CurrentClickFocus)
	{
		CurrentClickFocus->OnReset();
		CurrentClickFocus = nullptr;

		for (auto ClickComponent : ClickCompListToReset)
		{
			if (ClickComponent != nullptr)
			{
				ClickComponent->OnReset();
			}
		}

		ClickCompListToReset.Empty();
		UE_LOG(LogTemp, Warning, TEXT("Resetting Outlines. ClickCompListToReset.Num(): %i"), ClickCompListToReset.Num());
	}
}

void AGravityCharacter::UpdateOutlineUnderCursor()
{
	UClickInteractComponent* ClickComp = TryGetClickCompUnderCursor();
	if (ClickComp != nullptr)
	{
		ClickComp->ActivateHighlight(nullptr);
	}
}

UClickInteractComponent* AGravityCharacter::TryGetClickCompUnderCursor()
{
	// To prevent crash on loading new level
	if (!this->IsValidLowLevel())
	{
		return nullptr;
	}

	UWorld* World = GetWorld();

	if (World)
	{
		FHitResult Hit;
		bool bHit = World->GetFirstPlayerController()->GetHitResultUnderCursor(ECC_Visibility, false, Hit);
		if (bHit)
		{
			UActorComponent* ClickComp = Hit.GetActor()->GetComponentByClass(UClickInteractComponent::StaticClass());
			return Cast<UClickInteractComponent>(ClickComp);
		}
	}

	return nullptr;
}

EFocusType AGravityCharacter::GetClickFocusType(UClickInteractComponent* ClickFocus)
{
	if (ClickFocus == nullptr) { return EFocusType::Null; }
	if (ClickFocus->GetOwner() == this) { return EFocusType::Player; }
	return EFocusType::Object;
}

bool AGravityCharacter::IsComponentInLineOfSight(UActorComponent* Comp)
{
	if (Comp->GetOwner() == this)
	{
		return true;
	}

	FCollisionQueryParams CollisionParms;
	CollisionParms.AddIgnoredActor(this);

	FVector Start = GetActorLocation();
	FVector End = Comp->GetOwner()->GetActorLocation();
	//DrawDebugLine(GetWorld(), Start, End, FColor::Green, false, 0.5f);

	TArray<FHitResult> Hits;
	GetWorld()->LineTraceMultiByChannel(Hits, Start, End, ECC_Visibility, CollisionParms);
	for (auto Hit : Hits)
	{
		if (Hit.GetActor()->GetComponentByClass(UClickInteractComponent::StaticClass()) == nullptr)
		{
			UE_LOG(LogTemp, Warning, TEXT("Blocking player's sight! Hit: %s"), *Hit.GetActor()->GetName());
			return false;
		}
	}

	return true;

	//FHitResult Hit;
	//bool bHit = false;
	//UActorComponent* ClickComp = nullptr;
	//do
	//{
	//	ClickComp = nullptr;
	//	bHit = GetWorld()->LineTraceSingleByChannel(Hit, Start, End, ECC_GameTraceChannel8, CollisionParms);
	//	if (bHit)
	//	{
	//		UE_LOG(LogTemp, Warning, TEXT("IsComponentInLineOfSight() - Hit.GetActor(): %s"), *Hit.GetActor()->GetName());

	//		ClickComp = Hit.GetActor()->GetComponentByClass(UClickInteractComponent::StaticClass());
	//		if (ClickComp != nullptr)
	//		{
	//			UE_LOG(LogTemp, Warning, TEXT("IsComponentInLineOfSight() - ClickComp: %s"), *GetOwner()->GetName());
	//			CollisionParms.AddIgnoredActor(ClickComp->GetOwner());
	//		}
	//		else
	//		{
	//			UE_LOG(LogTemp, Warning, TEXT("IsComponentInLineOfSight() - ClickComp is nullptr"));
	//		}
	//	}
	//} while (ClickComp != nullptr);

	//if (bHit)
	//{
	//	DrawDebugPoint(GetWorld(), Hit.Location, 10, FColor::Red, false, 0.5f);
	//}

	//return !bHit;
}

bool AGravityCharacter::IsComponentWithinRange(UClickInteractComponent* Comp)
{
	if (ClickCompListToReset.Num() == 0)
	{
		TArray<UClickInteractComponent*> ClickComps = GetClickComponentsWithinRange();
		return ClickComps.Contains(Comp);
	}

	return ClickCompListToReset.Contains(Comp);
}

TArray<UClickInteractComponent*> AGravityCharacter::GetClickComponentsWithinRange()
{
	TArray<UClickInteractComponent*> ClickComponents;

	TArray<FHitResult> Hits;
	FCollisionShape MyColSphere = FCollisionShape::MakeSphere(ClickInteractRange);
	GetWorld()->SweepMultiByChannel(Hits, GetActorLocation(), GetActorLocation(), FQuat::Identity, ECC_GameTraceChannel8, MyColSphere);
	for (const FHitResult& Hit : Hits)
	{
		//DrawDebugLine(GetWorld(), GetActorLocation(), Hit.Location, FColor::Green, false, 1);
		//DrawDebugPoint(GetWorld(), Hit.Location, 10, FColor::Red, false, 1);

		UClickInteractComponent* HitComp = Cast<UClickInteractComponent>(Hit.GetActor()->GetComponentByClass(UClickInteractComponent::StaticClass()));
		if (HitComp != nullptr)
		{
			if (ClickComponents.Contains(HitComp)) { continue; }
			ClickComponents.Add(HitComp);
			ClickCompListToReset.Add(HitComp);
		}
	}

	return ClickComponents;
}

ESwapResult AGravityCharacter::CanSwapGravity(AActor* Actor1, AActor* Actor2)
{
	UGravitySwapComponent* Comp1 = Cast<UGravitySwapComponent>(Actor1->GetComponentByClass(UGravitySwapComponent::StaticClass()));
	UGravitySwapComponent* Comp2 = Cast<UGravitySwapComponent>(Actor2->GetComponentByClass(UGravitySwapComponent::StaticClass()));
	if (Comp1 == nullptr || Comp2 == nullptr)
	{
		return ESwapResult::Fail;
	}

	// same object
	if (Actor1 == Actor2)
	{
		return ESwapResult::Fail_SameObject;
	}

	// Is one of the focuses the player?
	bool bIsFocusPlayer = false;
	if (Cast<AGravityCharacter>(Comp1->GetOwner()) != nullptr
		|| Cast<AGravityCharacter>(Comp2->GetOwner()) != nullptr)
	{
		bIsFocusPlayer = true;
	}

	// One of the focuses is player but does not have Relic1 power.
	if (bHasRelic1 == false && bIsFocusPlayer == true)
	{
		UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : Player does not have a Relic1 power"));
		return ESwapResult::Fail_NoRelic1;
	}

	// both focuses are object but does not have Relic2 power
	if (bHasRelic2 == false && bIsFocusPlayer == false)
	{
		UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : Player does not have a Relic2 power"));
		return ESwapResult::Fail_NoRelic2;
	}

	// if one of them doesn't have UGravitySwapComponent
	if (Comp1 == nullptr || Comp2 == nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : One or both object don't have UGravitySwapComponent"));
		return ESwapResult::Fail;
	}

	// if one of them is not in player's line of sight
	if (!IsComponentInLineOfSight(Comp1) || !IsComponentInLineOfSight(Comp2))
	{
		UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : One of them is not in player's line of sight"));
		return ESwapResult::Fail_OutOfSight;
	}

	// if both has same gravity direction
	if (Comp1->GetFlipGravity() == Comp2->GetFlipGravity())
	{
		UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : Both has same gravity direction"));
		return ESwapResult::Fail_SameGravity;
	}

	// Is one on swap cooldown?
	//if (GravityComp1->IsOnCooldown() || GravityComp2->IsOnCooldown())
	//{
	//	UE_LOG(LogTemp, Warning, TEXT("Can't swap gravity! : Component on cooldown!"));
	//	return false;
	//}

	return ESwapResult::Success;
}

/// <returns>True if swap succeeded.</returns>
bool AGravityCharacter::SwapGravity(UClickInteractComponent* ClickComp1, UClickInteractComponent* ClickComp2)
{
	UGravitySwapComponent* GravityComp1 = Cast<UGravitySwapComponent>(GetComponentByInterface<UGravitySwappable>(ClickComp1->GetOwner()));
	UGravitySwapComponent* GravityComp2 = Cast<UGravitySwapComponent>(GetComponentByInterface<UGravitySwappable>(ClickComp2->GetOwner()));
	if (GravityComp1 == nullptr || GravityComp2 == nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Swap failed. %s or %s doesn't have a GravitySwap component attached."), *ClickComp1->GetOwner()->GetName(), *ClickComp2->GetOwner()->GetName());
		return false;
	}

	if (CanSwapGravity(GravityComp1->GetOwner(), GravityComp2->GetOwner()) != ESwapResult::Success)
	{
		UE_LOG(LogTemp, Warning, TEXT("Swap failed."));
		return false;
	}

	UE_LOG(LogTemp, Warning, TEXT("Swap succeeded."));
	GravityComp1->SetFlipGravity(!GravityComp1->GetFlipGravity());
	GravityComp2->SetFlipGravity(!GravityComp2->GetFlipGravity());
	return true;
}
#pragma endregion Click & Swap Interaction