#pragma once

#include "CoreMinimal.h"
#include "BasicEnemy.h"
#include "Components/BoxComponent.h"
#include "Components/ShapeComponent.h"
#include "GP2/GP2Character.h"
#include "Engine/TriggerBox.h"
#include <Runtime\Engine\Classes\Engine\TargetPoint.h>
#include "EnemySpawner.generated.h"

/**
 *
 */
USTRUCT(BlueprintType)
struct FEnemySpawn
{
	GENERATED_BODY()

public:
	FEnemySpawn() {}
	/** Object To Spawn*/
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Object Spawner")
		TSubclassOf<APawn> EnemyToSpawn;

	/**  Amount to spawn*/
	UPROPERTY(EditAnywhere, Category = "Object Spawner")
		int SpawnAmount;
};

UCLASS()
class GP2_API AEnemySpawner : public ATriggerBox
{
	GENERATED_BODY()

public:

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spawner")/** True is this encounter is triggered, if triggered it cant be re triggered. */
		bool bIsTriggered = false;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spawner")/** Struct for the enemy waves.*/
		TArray<FEnemySpawn> EnemyWave;

	/** Spawn Location, if not set, no enemies spawn. Tip: Will spawn element 1 of enemy list at element 1 of this array etc  */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spawn Points")
		TArray<AActor*> SpawnLocation;

	UPROPERTY(VisibleAnywhere, Category = "Collider")
		class UBoxComponent* BoxCollider;

	/* This value indicates the maximum number of times the spawner will trigger enemies.*/
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spawner")
		int32 MaxTriggers = 1;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Enemies")
		int KilledEnemies;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Enemies")
	int SpawnedEnemies;

	AEnemySpawner();

	UFUNCTION(BlueprintImplementableEvent)
		void OnEnemyDeath(APawn* Enemy);

	UFUNCTION()
		void OnBeginOverlap(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	UFUNCTION()
		void OnEndOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);

	void GenerateSpawnParameters(FActorSpawnParameters SpawnParams);

private:


};
