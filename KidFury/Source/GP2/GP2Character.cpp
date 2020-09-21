// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#include "GP2Character.h"
#include "UObject/ConstructorHelpers.h"
#include "Camera/CameraComponent.h"
#include "Components/DecalComponent.h"
#include "Components/CapsuleComponent.h"
#include "Components/SphereComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "GameFramework/PlayerController.h"
#include "GameFramework/SpringArmComponent.h"
#include "Components/SkeletalMeshComponent.h"
#include "HeadMountedDisplayFunctionLibrary.h"
#include "Materials/Material.h"
#include "Kismet/KismetMathLibrary.h" 
#include "Kismet/KismetSystemLibrary.h" 
#include "GP2PlayerController.h"
#include "Particles/ParticleSystemComponent.h" 
#include "Core/Weapons/Bat.h"
#include "Engine/World.h"

AGP2Character::AGP2Character()
{
	// Set size for player capsule
	GetCapsuleComponent()->InitCapsuleSize(42.f, 96.0f);

	// Don't rotate character to camera direction
	bUseControllerRotationPitch = false;
	bUseControllerRotationYaw = false;
	bUseControllerRotationRoll = false;

	// Configure character movement
	if (bUseInput)
	{
		GetCharacterMovement()->bOrientRotationToMovement = true; // Rotate character to moving direction
		GetCharacterMovement()->RotationRate = FRotator(0.f, 640.f, 0.f);
		GetCharacterMovement()->bConstrainToPlane = true;
		GetCharacterMovement()->bSnapToPlaneAtStart = true;
	}

	// Create a camera boom...
	CameraBoom = CreateDefaultSubobject<USpringArmComponent>(TEXT("CameraBoom"));
	CameraBoom->SetupAttachment(RootComponent);
	CameraBoom->SetUsingAbsoluteRotation(true); // Don't want arm to rotate when character does
	CameraBoom->TargetArmLength = 800.f;
	CameraBoom->SetRelativeRotation(FRotator(-60.f, 0.f, 0.f));
	CameraBoom->bDoCollisionTest = false; // Don't want to pull camera in when it collides with level

	// Create Run Paticle Emitter
	RunParticles = CreateDefaultSubobject<UParticleSystemComponent>(TEXT("RunParticles"));
	RunParticles->SetupAttachment(RootComponent);
	RunParticles->Deactivate();

	// Create a camera...
	TopDownCameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("TopDownCamera"));
	TopDownCameraComponent->SetupAttachment(CameraBoom, USpringArmComponent::SocketName);
	TopDownCameraComponent->bUsePawnControlRotation = false; // Camera does not rotate relative to arm

	// Activate ticking in order to update the cursor every frame.
	PrimaryActorTick.bCanEverTick = true;
	PrimaryActorTick.bStartWithTickEnabled = true;
}

void AGP2Character::Tick(float DeltaSeconds)
{
	Super::Tick(DeltaSeconds);
	RunParticles->DeactivaateNextTick();
	if (GetInputAxisValue("Horizontal") != 0 || GetInputAxisValue("Vertical") != 0)
	{
		RunParticles->Activate();
	}
	if (APlayerController* PC = Cast<APlayerController>(GetController()))
	{
		if (bUseInput)
		{
			if (bUseGamepad)
			{
				if (bShouldLookAtTarget) // Look in direction of right stick
				{
					FVector LookTarget(-GetInputAxisValue("R_Vertical"), GetInputAxisValue("R_Horizontal"), 0);
					if (LookTarget.Size() > 0.3f)
					{
						FRotator PlayerRot = UKismetMathLibrary::FindLookAtRotation(GetActorLocation(), GetActorLocation() + LookTarget * 1000.f);
						GetController()->SetControlRotation(PlayerRot);
					}
				}
			}
			else if (bShouldLookAtTarget) // Look at mouse cursor
			{
				FHitResult TraceHitResult;
				PC->GetHitResultUnderCursor(ECC_Visibility, true, TraceHitResult);
				FRotator PlayerRot = UKismetMathLibrary::FindLookAtRotation(GetActorLocation(), TraceHitResult.Location);
				GetController()->SetControlRotation(PlayerRot);

			}
		}
	}

}

void AGP2Character::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	//Binding player input and calling the appropriate Fuction.
	PlayerInputComponent->BindAxis("Turn", this, &AGP2Character::AddControllerYawInput);
	PlayerInputComponent->BindAxis("Vertical", this, &AGP2Character::MoveVertical);
	PlayerInputComponent->BindAxis("Horizontal", this, &AGP2Character::MoveHorizontal);
}

void AGP2Character::MoveVertical(float Axis)
{
	AddMovementInput({ 1,0,0 }, Axis);
}

void AGP2Character::MoveHorizontal(float Axis)
{
	AddMovementInput({ 0,1,0 }, Axis);
}

void AGP2Character::SetLookAtTarget(bool Value)
{
	bShouldLookAtTarget = Value;
	bUseControllerRotationYaw = Value;
}

AActor* AGP2Character::GetNearestEnemy(float Radius)
{
	// create tarray for hit results
	TArray<FHitResult> OutHits;

	// start and end locations
	FVector SweepStart = GetActorLocation();
	FVector SweepEnd = GetActorLocation();

	// create a collision sphere
	FCollisionShape MyColSphere = FCollisionShape::MakeSphere(Radius);

	// check if something got hit in the sweep
	bool isHit = GetWorld()->SweepMultiByObjectType(OutHits, SweepStart, SweepEnd, FQuat::Identity, ECC_Pawn, MyColSphere);

	AActor* Nearest = nullptr;
	float BestDistance = INFINITY;
	if (isHit)
	{
		// loop through TArray
		for (auto& Hit : OutHits)
		{
			if (Hit.Actor->IsA<AGP2Character>())
			{ continue; }
			float TempDistance = FVector::Distance(GetActorLocation(), Hit.Actor->GetActorLocation());
			if (BestDistance > TempDistance)
			{
				BestDistance = TempDistance;
				Nearest = Hit.GetActor();
			}
		}
	}
	return Nearest;
}

TArray<AActor*> AGP2Character::GetAllEnemysInRange(float Radius)
{
	// create tarray for hit results
	TArray<FHitResult> OutHits;

	// start and end locations
	FVector SweepStart = GetActorLocation();
	FVector SweepEnd = GetActorLocation();

	// create a collision sphere
	FCollisionShape MyColSphere = FCollisionShape::MakeSphere(Radius);

	// check if something got hit in the sweep
	bool isHit = GetWorld()->SweepMultiByObjectType(OutHits, SweepStart, SweepEnd, FQuat::Identity, ECC_Pawn, MyColSphere);

	TArray<AActor*> Actors;
	if (isHit)
	{
		// loop through TArray
		for (auto& Hit : OutHits)
		{
			//Ignore Player character
			if (Hit.Actor->IsA<AGP2Character>())
			{ continue; }
			Actors.Add(Hit.GetActor());
		}
	}
	return Actors;
}


AActor* AGP2Character::GetNearestEnemyInFront(float Radius, float CosAngle)
{
	// create tarray for hit results
	TArray<FHitResult> OutHits;

	// start and end locations
	const FVector SweepStart = GetActorLocation();
	const FVector SweepEnd = GetActorLocation();

	// create a collision sphere
	const FCollisionShape MyColSphere = FCollisionShape::MakeSphere(Radius);

	// check if something got hit in the sweep
	bool isHit = GetWorld()->SweepMultiByObjectType(OutHits, SweepStart, SweepEnd, FQuat::Identity, ECC_Pawn, MyColSphere);
	AActor* Nearest = nullptr;

	float BestAngle = CosAngle;
	if (isHit)
	{
		// loop through TArray
		for (auto& Hit : OutHits)
		{
			if (Hit.Actor->IsA<AGP2Character>())
			{ continue; }
			float TempAngle = GetActorForwardVector().CosineAngle2D(Hit.Actor->GetActorLocation() - GetActorLocation());
			UE_LOG(LogTemp, Warning, L"TempAngle : %f", TempAngle);
			if (BestAngle < TempAngle)
			{
				BestAngle = TempAngle;
				Nearest = Hit.GetActor();
			}
		}
	}
	return Nearest;
}
