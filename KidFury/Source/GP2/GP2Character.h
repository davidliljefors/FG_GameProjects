// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "Core/Projectiles/Projectile.h"
#include "GP2Character.generated.h"

UCLASS(Blueprintable)
class AGP2Character : public ACharacter
{
	GENERATED_BODY()

public:
	AGP2Character();
	virtual void Tick(float DeltaSeconds) override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;
	void MoveVertical(float Axis);
	void MoveHorizontal(float Axis);

	UFUNCTION(BlueprintCallable, Category = "Movement")
	void SetLookAtTarget(bool Value);

	/** Find closest enemy to player in radius
	* @return the enemy Actor
	*/
	UFUNCTION(BlueprintCallable, Category = "Enemies")
	AActor* GetNearestEnemy(float Radius = 500.f);

	/** Find all enemies in Radius
	* @return Array of all enemies
	*/
	UFUNCTION(BlueprintCallable, Category = "Enemies")
	TArray<AActor*> GetAllEnemysInRange(float Radius = 2000.f);

	/** Finds nearest enemy in a cone in front with the angle of @param CosAngle
	* @return the enemy Actor
	*/
	UFUNCTION(BlueprintCallable, Category = "Enemies")
	AActor* GetNearestEnemyInFront(float Radius = 500.f, float CosAngle = 0.f);

public:


	/** Returns TopDownCameraComponent subobject **/
	FORCEINLINE class UCameraComponent* GetTopDownCameraComponent() const { return TopDownCameraComponent; }
	/** Returns CameraBoom subobject **/
	FORCEINLINE class USpringArmComponent* GetCameraBoom() const { return CameraBoom; }

	/** Whether the player should look at the target or in direction of movement */
	UPROPERTY(BlueprintReadWrite, Category = "Movement")
		bool bShouldLookAtTarget = false;

	/** The total amount of time that the player is invulnerable after taking damage from the enemy.*/
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Invincibility Time")
		float InvincibilityTime = 1.0f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Input Switch")
		bool bUseInput = true;

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Input Switch")
		bool bUseGamepad = false;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "VFX")
		class UParticleSystemComponent* RunParticles = nullptr;


private:

	/** Top down camera */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
		class UCameraComponent* TopDownCameraComponent;

	/** Camera boom positioning the camera above the character */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
		class USpringArmComponent* CameraBoom;
};

