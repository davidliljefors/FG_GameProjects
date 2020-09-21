// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "BasicEnemy.h"
#include "BehaviorTree/Tasks/BTTask_BlackboardBase.h"
#include "BTTask_RotateTowardsTarget.generated.h"

/**
 * 
 */
UCLASS()
class GP2_API UBTTask_RotateTowardsTarget : public UBTTask_BlackboardBase
{
	GENERATED_BODY()
	
	virtual EBTNodeResult::Type ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory) override;

	ABasicEnemy* Self = nullptr;

	void RotateToTarget(FRotator CurrentRotation, FRotator TargetRotation);
};
