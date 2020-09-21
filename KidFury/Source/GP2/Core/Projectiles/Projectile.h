// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include <Components\SphereComponent.h>
#include <Components/StaticMeshComponent.h>
#include <GameFramework\ProjectileMovementComponent.h>

#include "Projectile.generated.h"

UCLASS()
class GP2_API AProjectile : public AActor
{
	GENERATED_BODY()

public:
	// Sets default values for this actor's properties
	AProjectile();

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Projectile Attributes")
		UStaticMeshComponent* StaticMeshComponent;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Projectile Attributes")
		USphereComponent* CollisionComponent;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Projectile Attributes")
		UProjectileMovementComponent* ProjectileMovementComponent;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Projectile Attributes", meta=(ExposeOnSpawn = "true"))
		/** Set the velocity of the projectile **/
		float Velocity = 1000.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Projectile Attributes")
		/** Set the lifespan of the projectile **/
		float LifeSpan;

	UPROPERTY(VisibleInstanceOnly, BlueprintReadWrite, Category = "Projectile Attributes")
		/** Bounce count */

		int32 BounceCount = 0;
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Projectile Attributes")
	/** Damage */
	float Damage = 25.f;


	UPROPERTY(VisibleInstanceOnly, BlueprintReadWrite, Category = "Projectile")
	AController* ProjectileInstigator = nullptr;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	virtual void OnConstruction(const FTransform& Transform) override;

private:
	UFUNCTION()
		/** This is execute when the projectile collide whit an object **/
		void OnHit(UPrimitiveComponent* HitComponent, AActor* OtherActor, UPrimitiveComponent* OtherComponent, FVector NormalImpulse, const FHitResult& Hit);

};
