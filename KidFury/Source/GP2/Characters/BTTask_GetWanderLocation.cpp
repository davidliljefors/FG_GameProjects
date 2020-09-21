// Fill out your copyright notice in the Description page of Project Settings.


#include "BTTask_GetWanderLocation.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "BehaviorTree/Blackboard/BlackboardKeyAllTypes.h"
#include "NavigationSystem/Public/NavigationSystem.h"

void UBTTask_GetWanderLocation::InitializeFromAsset(UBehaviorTree& Asset)
{
	Super::InitializeFromAsset(Asset);

	UBlackboardData* BBAsset = GetBlackboardAsset();
	if (BBAsset != nullptr)
	{
		MoveAroundPointOrigin.ResolveSelectedKey(*BBAsset);
	}
}

EBTNodeResult::Type UBTTask_GetWanderLocation::ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory)
{
	UBlackboardComponent* Blackboard = OwnerComp.GetBlackboardComponent();
	if (Blackboard == nullptr)
	{
		return EBTNodeResult::Failed;
	}

	FVector Origin = Blackboard->GetValue<UBlackboardKeyType_Vector>(MoveAroundPointOrigin.GetSelectedKeyID());

	FVector Destination;
	if (!UNavigationSystemV1::K2_GetRandomReachablePointInRadius(Blackboard->GetWorld(), Origin, Destination, Radius))
	{
		return EBTNodeResult::Failed;
	}

	return Blackboard->SetValue<UBlackboardKeyType_Vector>(BlackboardKey.SelectedKeyName, Destination) ? EBTNodeResult::Succeeded : EBTNodeResult::Failed;
}
