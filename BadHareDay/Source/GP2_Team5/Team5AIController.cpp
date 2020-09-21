// Fill out your copyright notice in the Description page of Project Settings.


#include "Team5AIController.h"
#include "Kismet/GameplayStatics.h"
#include "BehaviorTree/BlackboardComponent.h"

void ATeam5AIController::BeginPlay()
{
	Super::BeginPlay();

	if (AIBehavior != nullptr)
	{
		RunBehaviorTree(AIBehavior);

		APawn* PlayerPawn = UGameplayStatics::GetPlayerPawn(GetWorld(), 0);
				
		GetBlackboardComponent()->SetValueAsVector(TEXT("PlayerLocation"), PlayerPawn->GetActorLocation());
		GetBlackboardComponent()->SetValueAsObject(TEXT("Player"), PlayerPawn);

		SetFocus(PlayerPawn);
		UE_LOG(LogTemp, Warning, TEXT("SetFocus!!!!!!!!!!!"));
	}
}
