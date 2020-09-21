// Fill out your copyright notice in the Description page of Project Settings.


#include "BTTask_GetPlayerLocation.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "BehaviorTree/Blackboard/BlackboardKeyAllTypes.h"
#include <GameFramework/Actor.h>


EBTNodeResult::Type UBTTask_GetPlayerLocation::ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory)
{
	UBlackboardComponent* Blackboard = OwnerComp.GetBlackboardComponent();

	if (Blackboard == nullptr)
	{
		return EBTNodeResult::Failed;
	}


	AActor* Player = GetWorld()->GetFirstPlayerController()->GetPawn();

	if (Player == nullptr)
	{
		return EBTNodeResult::Failed;
	}
	return Blackboard->SetValue<UBlackboardKeyType_Object>(BlackboardKey.SelectedKeyName, Player) ? EBTNodeResult::Succeeded : EBTNodeResult::Failed;
}

