// Fill out your copyright notice in the Description page of Project Settings.


#include "ApproachInteractComponent.h"
#include "Blueprint/UserWidget.h"
#include "Components/WidgetComponent.h"

// Sets default values for this component's properties
UApproachInteractComponent::UApproachInteractComponent()
{
	PrimaryComponentTick.bCanEverTick = true;

}

// Called when the game starts
void UApproachInteractComponent::BeginPlay()
{
	Super::BeginPlay();

	WidgetComp = TryGetWidgetCompFromOwner();
	if (WidgetComp != nullptr)
	{
		WidgetComp->SetVisibility(false);
	}
}

// Called every frame
void UApproachInteractComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
}

void UApproachInteractComponent::Interact_Implementation()
{
	UE_LOG(LogTemp, Warning, TEXT("%s: Interact()"), *GetName());
	OnInteract.Broadcast();
}

void UApproachInteractComponent::InteractReleased_Implementation()
{
	UE_LOG(LogTemp, Warning, TEXT("%s: InteractReleased()"), *GetName());
	OnInteractReleased.Broadcast();
}

UWidgetComponent* UApproachInteractComponent::TryGetWidgetCompFromOwner()
{
	UActorComponent* Comp = GetOwner()->GetComponentByClass(UWidgetComponent::StaticClass());
	if (Comp == nullptr) { return nullptr; }

	return Cast<UWidgetComponent>(Comp);
}

void UApproachInteractComponent::ShowInteractionWidget()
{
	if (bShowInteractWidget == false) { return; }
	if (WidgetComp == nullptr) { return; }

	UE_LOG(LogTemp, Warning, TEXT("%s: ShowInteractionWidget()"), *GetName());
	WidgetComp->SetVisibility(true);
}

void UApproachInteractComponent::HideInteractionWidget()
{
	if (bShowInteractWidget == false) { return; }
	if (WidgetComp == nullptr) { return; }

	UE_LOG(LogTemp, Warning, TEXT("%s: HideInteractionWidget()"), *GetName());
	WidgetComp->SetVisibility(false);
}
