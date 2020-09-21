// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/PlayerController.h"
#include "GP2PlayerController.generated.h"

UCLASS()
class AGP2PlayerController : public APlayerController
{
	GENERATED_BODY()

public:
	AGP2PlayerController();

protected:
	// Begin PlayerController interface
	virtual void PlayerTick(float DeltaTime) override;
	virtual void SetupInputComponent() override;

public:
	void GetMouseWorldPosition(FVector& WorldLocation, FVector& WorldRotation);
};


