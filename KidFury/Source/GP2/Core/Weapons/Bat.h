// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "../Projectiles/Projectile.h"
#include "Bat.generated.h"


UCLASS()
class GP2_API ABat : public AActor
{
	GENERATED_BODY()


public: // Public members
	UPROPERTY(BlueprintReadOnly, Category = "Weapon")
	bool bIsCharging = false;

	/** The amount of charge added to all attacks */
	UPROPERTY(EditDefaultsOnly, Category = "Weapon")
		float BaseCharge = 0.5f;

	/** The amount of charge the meter starts at */
	UPROPERTY(EditDefaultsOnly, Category = "Weapon")
		float StartCharge = 1.0f;

	/** Max charge at end */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Weapon")
		float ChargeEnd = 2.f;

	/** Current charge in seconds */
	UPROPERTY(VisibleInstanceOnly, BlueprintReadWrite, Category = "Weapon")
		float CurrentCharge = 0.f;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Weapon")
	class UStaticMeshComponent* Mesh = nullptr;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Weapon")
	class UAudioComponent* AudioComponent = nullptr;

	/** Reference to the player holding the weapon */
	UPROPERTY(VisibleInstanceOnly, BlueprintReadWrite, Category = "Weapon")
		class AGP2Character* Player = nullptr;


	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Weapon")
	TSubclassOf<AProjectile> ProjectileClass;

protected:
	virtual void BeginPlay() override;
	virtual void Tick(float DeltaSeconds) override;

public: // Public Functions
	ABat();
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "Weapon")
	void StartPrimaryAttack();
	virtual void StartPrimaryAttack_Implementation();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "Weapon")
	void StopPrimaryAttack();
	virtual void  StopPrimaryAttack_Implementation();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "Weapon")
	void FireProjectile();
	virtual void FireProjectile_Implementation();
};
