#include "BTTask_FireAtPlayer.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "Components/CapsuleComponent.h"
#include "TimerManager.h"
#include "BehaviorTree/Blackboard/BlackboardKeyAllTypes.h"
#include <GP2\Core\Projectiles\Projectile.h>
#include <BehaviorTree/BTNode.h>

/** THIS CODE IS NOT  USEED ANYMORE, SEE THE BLUEPRINT BTTask_FireBallAtTarget instead*/

EBTNodeResult::Type UBTTask_FireAtPlayer::ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory)
{
	UBlackboardComponent* Blackboard = OwnerComp.GetBlackboardComponent();

	if (Blackboard == nullptr)
	{
		return EBTNodeResult::Failed;
	}

	Self = Cast<ABasicEnemy>(Blackboard->GetValueAsObject("SelfActor"));

	FVector TargetVector = Blackboard->GetValueAsVector("TargetPlayer");

	SpawnLocation = Cast<UArrowComponent>(Self->GetComponentByClass(UArrowComponent::StaticClass()));

	FireBullet(TargetVector);

	return EBTNodeResult::Succeeded;
}

void UBTTask_FireAtPlayer::FireBullet(FVector TargetLocation)
{
	AProjectile* Temp = GetWorld()->SpawnActor<AProjectile>(Bullet, SpawnLocation->GetComponentToWorld());
	Temp->CollisionComponent->IgnoreActorWhenMoving(Self, true);
	Self->GetCapsuleComponent()->IgnoreActorWhenMoving(Temp, true);
}

