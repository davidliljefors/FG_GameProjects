// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "ApproachInteract.h"
#include "Components/WidgetComponent.h"
#include "ApproachInteractComponent.generated.h"


UCLASS(Blueprintable, ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class GP2_TEAM5_API UApproachInteractComponent : public UActorComponent, public IApproachInteract
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UApproachInteractComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

public:

	UFUNCTION(BlueprintCallable)
	void ShowInteractionWidget() override;

	UFUNCTION(BlueprintCallable)
	void HideInteractionWidget() override;

private:
	DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnInteractDelegate);
	UPROPERTY(BlueprintAssignable, Category = "Interaction")
	FOnInteractDelegate OnInteract;

	UPROPERTY(BlueprintAssignable, Category = "Interaction")
	FOnInteractDelegate OnInteractReleased;

	UPROPERTY(EditAnywhere, Category = "Interaction")
	bool bShowInteractWidget = false;

	void Interact();
	virtual void Interact_Implementation();

	void InteractReleased();
	virtual void InteractReleased_Implementation();

	UWidgetComponent* TryGetWidgetCompFromOwner();

	UPROPERTY(EditAnywhere)
	UWidgetComponent* WidgetComp = nullptr;
};
