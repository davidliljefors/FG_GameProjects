// Fill out your copyright notice in the Description page of Project Settings.


#include "EnemySpawner.h"

AEnemySpawner::AEnemySpawner()
{
	bIsTriggered = false;

	GetCollisionComponent()->OnComponentBeginOverlap.AddDynamic(this, &AEnemySpawner::OnBeginOverlap);
	GetCollisionComponent()->OnComponentEndOverlap.AddDynamic(this, &AEnemySpawner::OnEndOverlap);

	if (MaxTriggers == 0)
	{
		MaxTriggers = 1;
	}
}

void AEnemySpawner::OnBeginOverlap(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	if (bIsTriggered)
	{
		return;
	}

	FActorSpawnParameters EnemySpawnParameters;

	GenerateSpawnParameters(EnemySpawnParameters);

	for (int EnemyIndex = 0; EnemyIndex < EnemyWave.Num(); EnemyIndex++)
	{
		for (int i = 0; i < EnemyWave[EnemyIndex].SpawnAmount; i++)
		{
			GetWorld()->SpawnActor<APawn>(EnemyWave[EnemyIndex].EnemyToSpawn, SpawnLocation[EnemyIndex]->GetActorTransform(), EnemySpawnParameters);

			SpawnedEnemies += EnemyWave[EnemyIndex].SpawnAmount; //Add spawned enemies to int
		}
	}

	if (OtherActor->IsA<AGP2Character>() && !bIsTriggered && MaxTriggers > 0)
	{
		bIsTriggered = true;
	}
}

void AEnemySpawner::OnEndOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex)
{
	bIsTriggered = true;
}

void AEnemySpawner::GenerateSpawnParameters(FActorSpawnParameters SpawnParams)
{
	SpawnParams.Owner = this;
	SpawnParams.SpawnCollisionHandlingOverride = ESpawnActorCollisionHandlingMethod::AdjustIfPossibleButAlwaysSpawn;
}
