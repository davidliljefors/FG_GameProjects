// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "LightEmitter.generated.h"

UCLASS()
class GP2_TEAM5_API ALightEmitter : public AActor
{
	GENERATED_BODY()
	
public:	
	ALightEmitter();

	virtual void BeginPlay() override;
	virtual void Tick(float DeltaTime) override;

	bool SendLaserCCW(FVector Start, int Bounces);
	bool SendLaserCW(FVector Start, int Bounces);

	bool SendLaserStraight(FVector Start, FVector Direction, int Bounces);

protected:
	UPROPERTY(Editanywhere, BlueprintReadWrite)
	float QuantizationLevel = 0.1F;

	UPROPERTY(Editanywhere)
	bool bIsCCW = false;

	int32 MaxBounces = 12;
};
