// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "CheckPointInterface.generated.h"

// This class does not need to be modified.
UINTERFACE(MinimalAPI)
class UCheckPointInterface : public UInterface
{
	GENERATED_BODY()
};

/**
 * 
 */
class GP2_TEAM5_API ICheckPointInterface
{
	GENERATED_BODY()

	// Add interface functions to this class. This is the class that will be inherited to implement this interface.
public:

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "C++ Interaction")
	void OnRest();
	virtual void OnRest_Implementation();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "C++ Interaction")
	void OnEnterCheckPoint(AActor* ActorCaller, AActor* CheckpointCaller);
	virtual void OnEnterCheckPoint_Implementation(AActor* ActorCaller, AActor* CheckpointCaller);

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "C++ Interaction")
	void OnFinnishlevel();
	virtual void OnFinnishLevel_Implementation();

};
