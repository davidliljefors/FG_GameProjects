// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#include "GP2PlayerController.h"
#include "Blueprint/AIBlueprintHelperLibrary.h"
#include "Runtime/Engine/Classes/Components/DecalComponent.h"
#include "HeadMountedDisplayFunctionLibrary.h"
#include "GP2Character.h"
#include "Engine/World.h"

AGP2PlayerController::AGP2PlayerController()
{
	bShowMouseCursor = true;
}

void AGP2PlayerController::PlayerTick(float DeltaTime)
{
	Super::PlayerTick(DeltaTime);
}

void AGP2PlayerController::SetupInputComponent()
{
	Super::SetupInputComponent();
}

void AGP2PlayerController::GetMouseWorldPosition(FVector& WorldLocation, FVector& WorldRotation)
{
	DeprojectMousePositionToWorld(WorldLocation, WorldRotation);
}
