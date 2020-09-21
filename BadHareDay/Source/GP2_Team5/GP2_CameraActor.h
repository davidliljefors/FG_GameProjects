// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "Math/UnrealMathUtility.h"
#include "Camera/CameraActor.h"
#include "GP2_CameraActor.generated.h"

/**
 * 
 */
UCLASS()
class GP2_TEAM5_API AGP2_CameraActor : public ACameraActor
{
	GENERATED_BODY()

public:
	
	AGP2_CameraActor();

	class UCameraComponent* CameraComponent;

protected:

	virtual void BeginPlay() override;

	virtual void Tick(float DeltaTime) override;

	FVector CameraLocation;
	AActor* TargetObject;
	FVector TargetActorPostion;
	FVector NewZoomValue;

public:

	UFUNCTION(BlueprintCallable, Category = "C++ Function")
		void ShakeCamera(int Strenght);

	UFUNCTION(BlueprintCallable, Category = "C++ Function")
		void FollowActor(AActor* TargetActor, float ZoomValue, float CameraSpeed);

private:

	


};
