// Fill out your copyright notice in the Description page of Project Settings.


#include "GP2_CameraActor.h"

AGP2_CameraActor::AGP2_CameraActor()
{
	CameraLocation = GetActorLocation();
}

void AGP2_CameraActor::BeginPlay()
{

}

void AGP2_CameraActor::Tick(float DeltaTime)
{
	
}

void AGP2_CameraActor::ShakeCamera(int Strenght)
{

}

void AGP2_CameraActor::FollowActor(AActor* TargetActor, float ZoomValue, float CameraSpeed)
{
	TargetActorPostion = TargetActor->GetActorLocation();

	TargetActorPostion = FVector(TargetActorPostion.Y * ZoomValue);

	SetActorLocation(FMath::Lerp(CameraLocation, TargetActorPostion, CameraSpeed), false, NULL);
}
