// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "Enums.h"
#include "ClickInteractComponent.generated.h"


template<class T>
UPrimitiveComponent* GetComponent(AActor* Actor)
{
	TArray<T*> Components;
	Actor->GetComponents<T>(Components);
	for (auto Comp : Components)
	{
		return Comp;
	}

	return nullptr;
}

UCLASS(ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class GP2_TEAM5_API UClickInteractComponent : public UActorComponent
{
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UClickInteractComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;


public:
	DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOutlineDelegate, EOutlineType, HightlightType);
	UPROPERTY(BlueprintAssignable, Category = "Interaction")
	FOutlineDelegate OnActivateHighlight;

	DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FSwapResultDelegate, ESwapResult, SwapResult);
	UPROPERTY(BlueprintAssignable, Category = "Interaction")
	FSwapResultDelegate OnTrySwappingGravity;

public:

	ESwapResult Clickable();
	void OnReset();

	UFUNCTION()
		void ActivateHighlight(UPrimitiveComponent* TouchedComponent);
	UFUNCTION()
		void DeactivateHighlight(UPrimitiveComponent* TouchedComponent);

	UFUNCTION()
		void OnMouseClick(UPrimitiveComponent* TouchedComponent, FKey ButtonPressed);

private:
	class AGravityCharacter* Player = nullptr;

	//bool bIsComponentWithinRange = false;

	bool HasSameGravity(AActor* Actor, AActor* OtherActor);

	UPrimitiveComponent* GetComponentToBindMouseEvents();
};