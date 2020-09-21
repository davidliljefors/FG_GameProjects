// Fill out your copyright notice in the Description page of Project Settings.


#include "BasicEnemy.h"

// Sets default values
ABasicEnemy::ABasicEnemy()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = false;

	EnemyCurrentHealth = EnemyMaxHealth;
}

// Called when the game starts or when spawned
void ABasicEnemy::BeginPlay()
{
	Super::BeginPlay();
}

// Called every frame
void ABasicEnemy::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
}

// Called to bind functionality to input
void ABasicEnemy::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
}

float ABasicEnemy::TakeDamage(float DamageAmout, struct FDamageEvent const& DamageEvent, class AController* EventInstigator, class AActor* DamageCauser)
{
	if (EnemyCurrentHealth >= 0)
	{
		EnemyCurrentHealth -= DamageAmout;
	}

	else if(EnemyCurrentHealth <= 0)
	{
		OnEnemyDeath();
		SetLifeSpan(0.01f);
	}
	return EnemyCurrentHealth;
}


