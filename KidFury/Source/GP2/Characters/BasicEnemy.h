// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "BasicEnemy.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnEnemyDeath);

UCLASS()
class GP2_API ABasicEnemy : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	ABasicEnemy();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	virtual void Tick(float DeltaTime) override;

	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	virtual float TakeDamage(float DamageAmout, struct FDamageEvent const& DamageEvent, class AController* EventInstigator, class AActor* DamageCauser) override;

	/** This event is called in blueprint for the designers when the enemy dies.*/
	UFUNCTION(BlueprintImplementableEvent)
	void OnEnemyDeath();

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Enemy Health")
	float EnemyMaxHealth = 10; /** This Value Sets the enemies current health on play.*/
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Enemy Health")
	float EnemyCurrentHealth = 10;

};
