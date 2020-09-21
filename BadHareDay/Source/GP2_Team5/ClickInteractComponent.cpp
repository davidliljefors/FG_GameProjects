// Fill out your copyright notice in the Description page of Project Settings.


#include "ClickInteractComponent.h"
#include "Kismet/GameplayStatics.h"
#include "GravityCharacter.h"
#include "Enums.h"
#include "Components/BoxComponent.h"
#include "GravitySwapComponent.h"
#include "TimerManager.h"
#include "Components/CapsuleComponent.h"

// Sets default values for this component's properties
UClickInteractComponent::UClickInteractComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UClickInteractComponent::BeginPlay()
{
	Super::BeginPlay();

	// ...

	Player = Cast<AGravityCharacter>(UGameplayStatics::GetPlayerPawn(GetWorld(), 0));

	UPrimitiveComponent* Comp = GetComponentToBindMouseEvents();
	if (Comp != nullptr)
	{
		Comp->OnBeginCursorOver.AddUniqueDynamic(this, &UClickInteractComponent::ActivateHighlight);
		Comp->OnEndCursorOver.AddUniqueDynamic(this, &UClickInteractComponent::DeactivateHighlight);
		Comp->OnClicked.AddUniqueDynamic(this, &UClickInteractComponent::OnMouseClick);
	}

	// RangeCheck timer
	////FTimerHandle RangeCheckTimer;
	////const auto OnCooldownFinished = [&]() {	bIsComponentWithinRange = Player->IsComponentWithinRange(this);	};
	////GetWorld()->GetTimerManager().SetTimer(RangeCheckTimer, OnCooldownFinished, 0.5f, true);

	//	lambda timer
	//FTimerHandle RangeCheckTimer;
	//GetWorld()->GetTimerManager().SetTimer(RangeCheckTimer, []() { UE_LOG(LogTemp, Warning, TEXT("Test")); }, 1, true);
}

// Called every frame
void UClickInteractComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...
}

UPrimitiveComponent* UClickInteractComponent::GetComponentToBindMouseEvents()
{
	// Mainly for player
	if (this->GetOwner() == Player)
	{
		UE_LOG(LogTemp, Warning, TEXT("Found UBoxComponent! Owner: %s, Mesh: %s"), *GetOwner()->GetName(), *Player->ClickBox->GetName());
		return Player->ClickBox;
	}

	// Mainly for Chandeliers
	UPrimitiveComponent* Comp = GetComponent<UBoxComponent>(GetOwner());
	if (Comp != nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Found UBoxComponent! Owner: %s, Mesh: %s"), *GetOwner()->GetName(), *Comp->GetName());
		return Comp;
	}

	// etc
	Comp = GetComponent<UStaticMeshComponent>(GetOwner());
	if (Comp != nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Found UStaticMeshComponent! Owner: %s"), *GetOwner()->GetName());
		return Comp;
	}

	//Comp = GetComponent<USkeletalMeshComponent>(GetOwner());
	//if (Comp != nullptr)
	//{
	//	UE_LOG(LogTemp, Warning, TEXT("Found USkeletalMeshComponent! Owner: %s"), *GetOwner()->GetName());
	//	return Comp;
	//}

	return Comp;
}

ESwapResult UClickInteractComponent::Clickable()
{
	UClickInteractComponent* CurrentClickFocus = Player->GetCurrentClickFocus();

	// has no ability to swap gravity between player + obj 
	if (!Player->HasRelic1())
	{
		if (Player->GetClickFocusType(CurrentClickFocus) == EFocusType::Player && Player->GetClickFocusType(this) == EFocusType::Object
			|| Player->GetClickFocusType(CurrentClickFocus) == EFocusType::Object && Player->GetClickFocusType(this) == EFocusType::Player)
		{
			UE_LOG(LogTemp, Warning, TEXT("Player doesn't have Relic1"));
			return ESwapResult::Fail_NoRelic1;
		}
	}

	// has no ability to swap gravity between obj + obj 
	if (!Player->HasRelic2())
	{
		if (Player->GetClickFocusType(CurrentClickFocus) == EFocusType::Object && Player->GetClickFocusType(this) == EFocusType::Object)
		{
			UE_LOG(LogTemp, Warning, TEXT("Player doesn't have Relic2"));
			return ESwapResult::Fail_NoRelic2;
		}
	}

	//// if this component CANNOT swap gravity with player's current focus
	//if (CurrentClickFocus != nullptr)
	//{
	//	if (Player->CanSwapGravity(this, CurrentClickFocus) == false)
	//	{
	//		return false;
	//	}
	//}

	// this component is not in player's line of sight
	if (Player->IsComponentInLineOfSight(this) == false)
	{
		UE_LOG(LogTemp, Warning, TEXT("This Actor ( %s ) is not in player's line of sight"), *GetOwner()->GetName());
		return ESwapResult::Fail_OutOfSight;
	}

	// this component is out of range
	if (Player->IsComponentWithinRange(this) == false)
	{
		UE_LOG(LogTemp, Warning, TEXT("This Actor ( %s ) is not within range."), *GetOwner()->GetName());
		return ESwapResult::Fail_OutOfRange;
	}

	// this component has same gravity
	if (Player->GetCurrentClickFocus() != nullptr && Player->GetCurrentClickFocus() != this)
	{
		if (HasSameGravity(this->GetOwner(), Player->GetCurrentClickFocus()->GetOwner()))
		{
			return ESwapResult::Fail_SameGravity;
		}
	}

	return ESwapResult::Success;
}

void UClickInteractComponent::OnReset()
{
	OnActivateHighlight.Broadcast(EOutlineType::None);
}

void UClickInteractComponent::ActivateHighlight(UPrimitiveComponent* TouchedComponent)
{
	UE_LOG(LogTemp, Warning, TEXT("BeginCursorOver: %s"), *GetOwner()->GetName());

	// if player doesn't have any abilities.
	if (!Player->HasRelic1() && !Player->HasRelic2()) { return; }

	// if player has CurrentClickFocus and this component is within range
	if (Player->GetCurrentClickFocus() != nullptr && Player->IsComponentWithinRange(this)) { return; }

	OnActivateHighlight.Broadcast(Clickable() == ESwapResult::Success ? EOutlineType::Hover : EOutlineType::NonSwappable);
}

void UClickInteractComponent::DeactivateHighlight(UPrimitiveComponent* TouchedComponent)
{
	UE_LOG(LogTemp, Warning, TEXT("EndCursorOver: %s"), *GetOwner()->GetName());

	// if player has CurrentClickFocus and this component is within range
	if (Player->GetCurrentClickFocus() != nullptr && Player->IsComponentWithinRange(this)) { return; }

	OnActivateHighlight.Broadcast(EOutlineType::None);
}

void UClickInteractComponent::OnMouseClick(UPrimitiveComponent* TouchedComponent, FKey ButtonPressed)
{
	UE_LOG(LogTemp, Warning, TEXT("Clicked: %s"), *GetOwner()->GetName());

	// get swap result
	if (Player->GetCurrentClickFocus() != nullptr)
	{
		OnTrySwappingGravity.Broadcast(Player->CanSwapGravity(this->GetOwner(), Player->GetCurrentClickFocus()->GetOwner()));
	}

	if (Clickable() != ESwapResult::Success)
	{
		Player->OnResetClickAction();
		return;
	}

	// Player has CurrentClickFocus. Trying gravity swap.
	if (Player->GetCurrentClickFocus() != nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("Clicked: %s. Player has CurrentClickFocus. Trying gravity swap."), *GetOwner()->GetName());
		Player->SwapGravity(this, Player->GetCurrentClickFocus());

		UE_LOG(LogTemp, Warning, TEXT("Resetting CurrentClickFocus and deactivating outlines on components within range."));
		Player->OnResetClickAction();
	}
	// Player doesn't have CurrentClickFocus. Setting this one as CurrentClickFocus.
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Clicked: %s. Player doesn't have CurrentClickFocus. Setting this one as CurrentClickFocus."), *GetOwner()->GetName());
		Player->SetCurrentClickFocus(this);

		UE_LOG(LogTemp, Warning, TEXT("Getting components within range and activate their outline."));
		TArray<UClickInteractComponent*> ClickComponents = Player->GetClickComponentsWithinRange();
		for (auto ClickComponent : ClickComponents)
		{
			UE_LOG(LogTemp, Warning, TEXT("Actor within range: %s."), *ClickComponent->GetOwner()->GetName());
			// This component is clicked
			if (ClickComponent == this)
			{
				ClickComponent->OnActivateHighlight.Broadcast(EOutlineType::Selected);
			}
			else
			{
				// if ClickComponent has same gravity with this
				bool bSwappable = Player->CanSwapGravity(this->GetOwner(), ClickComponent->GetOwner()) == ESwapResult::Success;
				EOutlineType OutlineType = bSwappable ? EOutlineType::Swappable : EOutlineType::NonSwappable;
				ClickComponent->OnActivateHighlight.Broadcast(OutlineType);
			}
		}
	}
}

bool UClickInteractComponent::HasSameGravity(AActor* Actor, AActor* OtherActor)
{
	if (Actor == nullptr || OtherActor == nullptr) { return false; }

	UGravitySwapComponent* GravityComp1 = Cast<UGravitySwapComponent>(GetComponentByInterface<UGravitySwappable>(Actor));
	UGravitySwapComponent* GravityComp2 = Cast<UGravitySwapComponent>(GetComponentByInterface<UGravitySwappable>(OtherActor));
	if (GravityComp1 == nullptr || GravityComp2 == nullptr) { return false; }

	return GravityComp1->GetFlipGravity() == GravityComp2->GetFlipGravity();
}

