// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Math/Vector.h"
#include "Engine/World.h"
#include "BasicEnemy.h"
#include "Components/ArrowComponent.h"
#include "BehaviorTree/Tasks/BTTask_BlackboardBase.h"
#include "BTTask_FireAtPlayer.generated.h"

/**
 * 
 */
UCLASS()
class GP2_API UBTTask_FireAtPlayer : public UBTTask_BlackboardBase
{
	GENERATED_BODY()
public:

    virtual EBTNodeResult::Type ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory) override;

    UFUNCTION()
    void FireBullet(FVector TargetLocation);

    ABasicEnemy* Self = nullptr;

   // FTimerHandle FireHandle;
   // FTimerDelegate FireDelegate;
   //
   // int ShotsFired = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Bullet")
    TSubclassOf<AActor> Bullet;

    UPROPERTY(VisibleInstanceOnly, BlueprintReadWrite, Category = "Projectile Spawn Location")
    UArrowComponent* SpawnLocation = nullptr;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Fire Rate")
    float FireRate = 1.0f;

};
