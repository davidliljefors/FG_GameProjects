// Fill out your copyright notice in the Description page of Project Settings.

#include "BTTask_RotateTowardsTarget.h"

#include "BTTask_FireAtPlayer.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "Components/CapsuleComponent.h"
#include "BehaviorTree/Blackboard/BlackboardKeyAllTypes.h"

EBTNodeResult::Type UBTTask_RotateTowardsTarget::ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory)
{
	UBlackboardComponent* Blackboard = OwnerComp.GetBlackboardComponent();

	if (Self == nullptr)
	{
		Self = Cast<ABasicEnemy>(Blackboard->GetValueAsObject("SelfActor"));
	}

	if (Blackboard == nullptr)
	{
		return EBTNodeResult::Failed;
	}

	FRotator CurrentRot = Self->GetActorRotation();

	return EBTNodeResult::Succeeded;
}

void UBTTask_RotateTowardsTarget::RotateToTarget(FRotator CurrentRotation, FRotator TargetRotation)
{
	
}
