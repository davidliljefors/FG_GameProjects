// Fill out your copyright notice in the Description page of Project Settings.


#include "LightEmitter.h"
#include "Engine/World.h"

#include "DrawDebugHelpers.h"

ALightEmitter::ALightEmitter()
{
	PrimaryActorTick.bCanEverTick = true;
}

// Called when the game starts or when spawned
void ALightEmitter::BeginPlay()
{
	Super::BeginPlay();
}

// Called every frame
void ALightEmitter::Tick(float DeltaTime)
{
	QuantizationLevel = FMath::Max(QuantizationLevel, 0.01f);
	Super::Tick(DeltaTime);
	if (bIsCCW)
	{
		SendLaserCCW(GetActorLocation(), 0);
	}
	else
	{
		SendLaserCW(GetActorLocation(), 0);
	}
}


bool ALightEmitter::SendLaserCCW(FVector Start, int Bounces)
{
	if (Bounces > MaxBounces) return false;

	float DistanceFromCenter = Start.Size();
	Start.Normalize();

	auto angle = FMath::Atan2(Start.Y, Start.Z);

	for (float i = angle; i < 1.9F * PI + angle; i += QuantizationLevel)
	{
		const float StartY = FMath::Sin(i) * DistanceFromCenter;
		const float StartZ = FMath::Cos(i) * DistanceFromCenter;
		const float EndY = FMath::Sin(i + QuantizationLevel) * DistanceFromCenter;
		const float EndZ = FMath::Cos(i + QuantizationLevel) * DistanceFromCenter;

		const FVector StartPoint = { 0.0F, StartY, StartZ };
		const FVector EndPoint = { 0.0F, EndY, EndZ };

		FHitResult Hit;
		bool bHitSomething = GetWorld()->LineTraceSingleByChannel(Hit, StartPoint, EndPoint, ECollisionChannel::ECC_Visibility);
		if (bHitSomething)
		{
			DrawDebugLine(GetWorld(), StartPoint, Hit.ImpactPoint, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
			
			FVector Incidence = (StartPoint - EndPoint).GetSafeNormal();

			FQuat Rot = FQuat::FindBetweenNormals(Incidence, Hit.Normal);
			FVector OutVector = Rot * Hit.Normal;

			return SendLaserStraight(Hit.ImpactPoint, OutVector, Bounces + 1);

		}
		DrawDebugLine(GetWorld(), StartPoint, EndPoint, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
	}
	return false;
}

bool ALightEmitter::SendLaserCW(FVector Start, int Bounces)
{
	if (Bounces > MaxBounces) return false;

	float DistanceFromCenter = Start.Size();
	Start.Normalize();

	float Angle = FMath::Atan2(Start.Y, Start.Z);

	for (float i = Angle; i > Angle - 1.9F * PI; i -= QuantizationLevel)
	{
		const float StartY = FMath::Sin(i) * DistanceFromCenter;
		const float StartZ = FMath::Cos(i) * DistanceFromCenter;
		const float EndY = FMath::Sin(i - QuantizationLevel) * DistanceFromCenter;
		const float EndZ = FMath::Cos(i - QuantizationLevel) * DistanceFromCenter;

		const FVector StartPoint = { 0.0F, StartY, StartZ };
		const FVector EndPoint = { 0.0F, EndY, EndZ };

		FHitResult Hit;
		bool bHitSomething = GetWorld()->LineTraceSingleByChannel(Hit, StartPoint, EndPoint, ECollisionChannel::ECC_Visibility);
		if (bHitSomething)
		{
			DrawDebugLine(GetWorld(), StartPoint, Hit.ImpactPoint, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
			
			FVector Incidence = (StartPoint - EndPoint).GetSafeNormal();

			FQuat Rot = FQuat::FindBetweenNormals(Incidence , Hit.Normal);
			FVector OutVector = Rot * Hit.Normal;

			return SendLaserStraight(Hit.ImpactPoint, OutVector, Bounces + 1);
		}
		DrawDebugLine(GetWorld(), StartPoint, EndPoint, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
	}
	return false;
}

bool ALightEmitter::SendLaserStraight(FVector Start, FVector Direction, int Bounces)
{
	if (Bounces > MaxBounces) return false;

	FVector End = Start + 1000.F * Direction;

	FHitResult Hit;
	bool bHitSomething = GetWorld()->LineTraceSingleByChannel(Hit, Start, End, ECollisionChannel::ECC_Visibility);
	if (bHitSomething)
	{
		DrawDebugLine(GetWorld(), Start, Hit.ImpactPoint, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
		FVector HitDirection = FVector::CrossProduct(Hit.ImpactPoint, Hit.Normal);

		UE_LOG(LogTemp, Warning, TEXT("Send up hit  X =  %f"), HitDirection.X);
		if (HitDirection.X > 0.0F)
		{
			return SendLaserCW(Hit.ImpactPoint, Bounces + 1);
		}
		else
		{
			return SendLaserCCW(Hit.ImpactPoint, Bounces + 1);
		}
	}
	DrawDebugLine(GetWorld(), Start, End, FColor(255, 0, 0), false, 0.1f, 0, 3.f);
	return false;
}
