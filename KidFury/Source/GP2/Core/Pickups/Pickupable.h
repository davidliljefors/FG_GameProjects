// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Components/SphereComponent.h"
#include "Components/StaticMeshComponent.h"
#include "GameFramework/RotatingMovementComponent.h"
#include "GP2/GP2Character.h"
#include "Components/PrimitiveComponent.h"
#include "Pickupable.generated.h"

UCLASS()
class GP2_API APickupable : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	APickupable();

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Pickupable Attributes")
		UStaticMeshComponent* StaticMeshComponent;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Pickupable Attributes")
		USphereComponent* CollisionComponent;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Pickupable Attributes")
		URotatingMovementComponent* RotatingMovementComponent;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Pickupable Attributes")
		/**Set if the object should be tossed up in the air or not (default = true)*/
		bool bTossIntoAir = true;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Pickupable Attributes")
		/**Set the speed how fast the object should rotate*/
		float RotateSpeed = 50;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	void TossObjectIntoAir(bool TossIntoAir = true);
};
