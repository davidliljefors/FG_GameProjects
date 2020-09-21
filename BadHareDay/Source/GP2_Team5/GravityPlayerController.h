// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/PlayerController.h"
#include "Enums.h"
#include "GravityPlayerController.generated.h"

/**
 * 
 */
UCLASS()
class GP2_TEAM5_API AGravityPlayerController : public APlayerController
{
	GENERATED_BODY()

protected:
	virtual void BeginPlay() override;

public:

	UFUNCTION(BlueprintCallable)
	void ClearLevel(EWinScreen ClearScreen);

private:
	UPROPERTY(EditAnywhere)
	TSubclassOf<class UUserWidget> Level1ClearScreenClass;

	UPROPERTY(EditAnywhere)
	TSubclassOf<class UUserWidget> Level2ClearScreenClass;
};
