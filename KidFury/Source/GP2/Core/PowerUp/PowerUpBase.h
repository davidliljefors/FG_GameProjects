// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "PowerUpBase.generated.h"


UCLASS(Blueprintable, ClassGroup=(PowerUp), meta=(BlueprintSpawnableComponent))
class GP2_API UPowerUpBase : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UPowerUpBase();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:	
	UFUNCTION(BlueprintCallable, Category = "PowerUp")
		void ExecuteDash(AActor* Owner, float DashForce);
		
};
