// Fill out your copyright notice in the Description page of Project Settings.

#include "GravitySwapComponent.h"
#include "Kismet/KismetMathLibrary.h"
#include <../Plugins/Runtime/ApexDestruction/Source/ApexDestruction/Public/DestructibleComponent.h>
#include "ClickInteractComponent.h"

UGravitySwapComponent::UGravitySwapComponent()
{
	PrimaryComponentTick.bCanEverTick = bCanEverTick;
}

void UGravitySwapComponent::BeginPlay()
{
	SetComponentTickEnabled(bCanEverTick);
	Super::BeginPlay();

	TArray<UStaticMeshComponent*> Components;
	GetOwner()->GetComponents<UStaticMeshComponent>(Components);
	for (int32 i = 0; i < Components.Num(); i++)
	{
		PhysicsComp = Components[i];
		return;
	}

	TArray<UDestructibleComponent*> DestructibleComponents;
	GetOwner()->GetComponents<UDestructibleComponent>(DestructibleComponents);
	for (int32 i = 0; i < DestructibleComponents.Num(); i++)
	{
		UE_LOG(LogTemp, Warning, TEXT("Found a destructible"));
		PhysicsComp = DestructibleComponents[i];
		return;
	}

}

void UGravitySwapComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	if (PhysicsComp && PhysicsComp->IsSimulatingPhysics())
	{
		FVector GravityDirection = UKismetMathLibrary::GetDirectionUnitVector(GetOwner()->GetActorLocation(), GravityPoint);
		float Force = GravityAcceleration * PhysicsComp->GetMass();

		if (bFlipGravity)
		{
			PhysicsComp->AddForce(GravityDirection * Force * -1.f, NAME_None, true);
		}
		else
		{
			PhysicsComp->AddForce(GravityDirection * Force, NAME_None, true);
		}
	}
}

bool UGravitySwapComponent::GetFlipGravity() const
{
	return bFlipGravity;
}


void UGravitySwapComponent::SetFlipGravity(bool bNewGravity)
{
	float TimeElapsed = 0.0f;

	bIsOnCooldown = true;

	const auto OnCooldownFinished = [&]()
	{
		bIsOnCooldown = false;
		// check object under mouse cursor
		FHitResult Hit;
		if (GetWorld()->GetFirstPlayerController()->GetHitResultUnderCursor(ECC_Visibility, false, Hit))
		{
			// try get click comp
			UClickInteractComponent* ClickCompUnderCursor = Cast<UClickInteractComponent>(Hit.GetActor()->GetComponentByClass(UClickInteractComponent::StaticClass()));
			if (ClickCompUnderCursor != nullptr)
			{
				// only do this on the one under mouse cursor
				if (ClickCompUnderCursor->GetOwner() == this->GetOwner())
				{
					// update outline
					ClickCompUnderCursor->ActivateHighlight(nullptr);
				}
			}
		}
	};

	UWorld* World = GetWorld();
	if (World)
	{
		World->GetTimerManager().SetTimer(CooldownTimerHandle, OnCooldownFinished, SwapCooldown, false, SwapCooldown);
	}

	bFlipGravity = bNewGravity;
	OnFlipGravity.Broadcast(bNewGravity);
}
