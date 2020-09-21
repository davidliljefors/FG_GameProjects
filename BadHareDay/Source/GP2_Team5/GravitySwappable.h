// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "GravitySwappable.generated.h"

// This class does not need to be modified.
UINTERFACE(MinimalAPI, BlueprintType, meta=(CannotImplementInterfaceInBlueprint))
class UGravitySwappable : public UInterface
{
	GENERATED_BODY()

};

/**
 * 
 */
class GP2_TEAM5_API IGravitySwappable
{
	GENERATED_BODY()


public:

	UFUNCTION(BlueprintCallable)
	virtual bool GetFlipGravity() const = 0;

	UFUNCTION(BlueprintCallable)
	virtual void SetFlipGravity(bool bNewGravity) = 0;
};