// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Enums.h"
#include "Collectible.generated.h"


UCLASS()
class GP2_TEAM5_API ACollectible : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ACollectible();
	virtual void BeginPlay() override;
	virtual void Tick(float DeltaTime);

	ECollectibleType GetType() const { return Type; }
	int32 GetCount() const { return Count; }

	UFUNCTION(BlueprintImplementableEvent, Category ="Collectible")
	void OnPickedUp(class AGravityCharacter* PickedBy);

	UFUNCTION()
	void OnComponentBeginOverlap(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor,
		UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

protected:
	
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category = "Collectible")
	ECollectibleType Type = ECollectibleType::Coin;

	// Amount of items added when collected
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category = "Collectible")
	int32 Count = 1;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category ="Collectible")
	class URotatingMovementComponent* RotatingMovementCmp = nullptr;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Collectible")
	class USphereComponent* SphereCollision = nullptr;

	UPROPERTY(VisibleAnywhere, BlueprintReadWrite, Category = "Collectible")
	class UStaticMeshComponent* Mesh = nullptr;

};
