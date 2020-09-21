// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "BehaviorTree/Tasks/BTTask_BlackboardBase.h"
#include "BTTask_GetWanderLocation.generated.h"

/**
 * 
 */
UCLASS()
class GP2_API UBTTask_GetWanderLocation : public UBTTask_BlackboardBase
{
	GENERATED_BODY()

public:
    UPROPERTY(EditAnywhere, Category = "Blackboard")
    FBlackboardKeySelector MoveAroundPointOrigin;

    UPROPERTY(EditAnywhere, Category = "Blackboard")
    float Radius = 500.0f; /** Search Radius */

    virtual void InitializeFromAsset(UBehaviorTree& Asset) override;

    virtual EBTNodeResult::Type ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory) override;
};
