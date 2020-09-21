// Fill out your copyright notice in the Description page of Project Settings.

#include "Bat.h"
#include "../../GP2Character.h"
#include "Kismet/GameplayStatics.h" 
#include "Components/AudioComponent.h" 
#include "Components/StaticMeshComponent.h" 
#include "Engine/World.h" 

// Sets default values
ABat::ABat()
{
	Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Mesh"));
	RootComponent = Mesh;
	AudioComponent = CreateDefaultSubobject<UAudioComponent>(TEXT("Audio"));
	PrimaryActorTick.bCanEverTick = true;
	PrimaryActorTick.bStartWithTickEnabled = false;
}

// Called when the game starts or when spawned
void ABat::BeginPlay()
{
	Super::BeginPlay();
}

void ABat::Tick(float DeltaSeconds)
{
	if (bIsCharging)
	{
		if (CurrentCharge < ChargeEnd)
		{
			CurrentCharge += DeltaSeconds;
		}
		else
		{
		}
	}
}

void ABat::StartPrimaryAttack_Implementation()
{
	SetActorTickEnabled(true);
	CurrentCharge = StartCharge;
	bIsCharging = true;
}

void ABat::StopPrimaryAttack_Implementation()
{
	if (bIsCharging)
	{
		FireProjectile();
		SetActorTickEnabled(false);
	}
	bIsCharging = false;
}

void ABat::FireProjectile_Implementation()
{
	/*if (Player)
	{
		if (Player->ProjectilesInRange.Num() > 0)
		{
			AProjectile* tmp = Player->ProjectilesInRange[0];
			if (tmp)
			{
				AudioComponent->Play();
				tmp->SetActorHiddenInGame(true);
				tmp->SetActorEnableCollision(false);
				tmp->SetLifeSpan(0.1f);

				if (APlayerController* PC = Cast<APlayerController>(Player->GetController()))
				{
					FHitResult TraceHitResult;
					PC->GetHitResultUnderCursor(ECC_Visibility, true, TraceHitResult);
					FTransform Spawn = GetActorTransform();
					FVector Diff = TraceHitResult.Location - GetActorLocation();
					Diff.Rotation();
					Spawn.SetRotation(Diff.Rotation().Quaternion());
					AProjectile * ProjectileInstance = GetWorld()->SpawnActorDeferred<AProjectile>(ProjectileClass, Spawn, Player, Player, ESpawnActorCollisionHandlingMethod::AlwaysSpawn);
					if (ProjectileInstance)
					{
						ProjectileInstance->ProjectileInstigator = Player->GetController();
						Player->MoveIgnoreActorAdd(ProjectileInstance);
						ProjectileInstance->CollisionComponent->IgnoreActorWhenMoving(Player, true);
						ProjectileInstance->Velocity *= BaseCharge + (CurrentCharge * CurrentCharge * CurrentCharge);
						UGameplayStatics::FinishSpawningActor(ProjectileInstance, Spawn);
					}
				}
			}
		}
	}*/
}

