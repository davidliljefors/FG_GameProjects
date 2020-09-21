// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "ApproachInteract.h"
#include "GravitySwappable.h"
#include "ClickInteract.h"
#include "ClickInteractComponent.h"
#include "Collectible.h"
#include "Enums.h"
#include "ApproachInteractComponent.h"
#include "DrawDebugHelpers.h"
#include "GravitySwapComponent.h"
#include "GravityCharacter.generated.h"

template<class T>
UActorComponent* GetComponentByInterface(AActor* Actor)
{
	if (Actor == nullptr) { return nullptr; }
	TArray<UActorComponent*> Components = Actor->GetComponentsByInterface(T::StaticClass());
	for (UActorComponent* Comp : Components)
	{
		if (Comp->GetClass()->ImplementsInterface(T::StaticClass()))
		{
			return Comp;
		}
	}
	return nullptr;
}

//--- forward declarations ---
class UGravityMovementComponent;

UCLASS()
class GP2_TEAM5_API AGravityCharacter : public ACharacter, public IGravitySwappable
{
	GENERATED_BODY()


public:
	// Sets default values for this character's properties
	AGravityCharacter(const FObjectInitializer& ObjectInitializer);

	// IGravitySwappable
	virtual bool GetFlipGravity() const override;
	virtual void SetFlipGravity(bool bNewGravity) override;

	virtual void AddCollectible(ACollectible* Collectible);

protected:
	virtual void BeginPlay() override;
	virtual void Tick(float DeltaTime) override;
	void MoveRight(float Val);
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	// Sets the point the character gravitates towards
	UFUNCTION(BlueprintCallable)
	void SetGravityTarget(FVector NewGravityPoint);

	UFUNCTION(BlueprintImplementableEvent, Category = "Collectible")
	void OnCollectibleAdded(ECollectibleType Type, int32 NewCount);

	UFUNCTION(BlueprintPure)
	UGravityMovementComponent* GetGravityMovementComponent() const { return CachedGravityMovementyCmp; }
	FORCEINLINE class UCameraComponent* GetSideViewCameraComponent() const { return SideViewCameraComponent; }
	FORCEINLINE class USpringArmComponent* GetCameraBoom() const { return CameraBoom; }

	// -- Member variables --
protected:
	UPROPERTY(SaveGame, BlueprintReadWrite, Category = "GravityCharacter|SaveData")
	TMap<ECollectibleType, int32> Collectibles;

	UGravityMovementComponent* CachedGravityMovementyCmp = nullptr;

	UPROPERTY(EditAnywhere, Category = "GravityCharacter|Gravity")
	FVector GravityPoint {};

	/* The rate at which gravity changes from old to new target */
	UPROPERTY(EditAnywhere, Category = "GravityCharacter|Gravity")
	float GravityChangeSpeed = 25.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "GravityCharacter|Gravity")
	bool bFlipGravity = false;

	UPROPERTY(EditDefaultsOnly, Category = "GravityCharacter|Camera")
	bool bRotateCameraToPlayer = false;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera)
	class UCameraComponent* SideViewCameraComponent;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera)
	class USpringArmComponent* CameraBoom;

#pragma region Movement
	
	void Jump();

	UFUNCTION(BlueprintPure, Category = "GravityCharacter|Jump")
	bool IsJumping();

	float BaseWalkSpeed = 100.0f;
	UPROPERTY(EditDefaultsOnly, Category = "GravityCharacter | Movement")
	float PushSpeed = 300.f;

#pragma endregion

#pragma region Interaction
protected:

	UPROPERTY(EditAnywhere, Category = "GravityCharacter|Interaction")
	class UBoxComponent* InteractBox;

	UFUNCTION()
	void OnInteractBoxBeginOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);
	UFUNCTION()
	void OnInteractBoxEndOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "GravityCharacter|Interaction")
	float ClickInteractRange = 700.f;

private:
	UPROPERTY(EditDefaultsOnly, Category = "GravityCharacter|Interaction")
	float AutoReleaseBoxDistance = 200.f;

protected:
	// Approach Interact
	void OnApproachInteract();
	void OnApproachInteractReleased();
	UApproachInteractComponent* TryGetApproachInteractableComp();
	UApproachInteractComponent* ApproachInteractableComp = nullptr;

	UFUNCTION(BlueprintCallable, Category = "GravityCharacter|Interaction")
	void ResetWalkSpeed();

	// Click Interact
	void OnClick();
	void UpdateOutlineUnderCursor();
	UClickInteractComponent* TryGetClickCompUnderCursor();
	UClickInteractComponent* CurrentClickFocus = nullptr;
	TArray<UClickInteractComponent*> ClickCompListToReset;

	// Grab
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "GravityCharacter|Interaction")
	UApproachInteractComponent* CurrentGrabbingBox = nullptr;

	// Relic
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "GravityCharacter|Interaction")
	bool bHasRelic1 = false;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "GravityCharacter|Interaction")
	bool bHasRelic2 = false;

public:
	// Click
	UPROPERTY(EditAnywhere, Category = "GravityCharacter|Interaction")
	class UBoxComponent* ClickBox;
	float GetClickInteractRange() { return ClickInteractRange; }
	EFocusType GetClickFocusType(UClickInteractComponent* ClickFocus);
	bool IsComponentWithinRange(UClickInteractComponent* Comp);
	TArray<UClickInteractComponent*> GetClickComponentsWithinRange();
	UClickInteractComponent* GetCurrentClickFocus() { return CurrentClickFocus; }
	void SetCurrentClickFocus(UClickInteractComponent* NewFocus) { CurrentClickFocus = NewFocus; }
	//void SetClickCompListToReset(TArray<UClickInteractComponent*> NewList) { ClickCompListToReset = NewList; }
	void OnResetClickAction();

	// Gravity
	ESwapResult CanSwapGravity(AActor* Actor1, AActor* Actor2);
	bool SwapGravity(UClickInteractComponent* ClickComp1, UClickInteractComponent* ClickComp2);
	bool IsComponentInLineOfSight(UActorComponent* Comp);

	// Grab
	UFUNCTION(BlueprintPure)
	bool IsGrabbing() { return CurrentGrabbingBox != nullptr; }
	UFUNCTION(BlueprintCallable)
	void ResetCurrentGrabbingBox() { CurrentGrabbingBox = nullptr; }

	// Relic
	UFUNCTION(BlueprintPure)
	bool HasRelic1() { return bHasRelic1; }
	UFUNCTION(BlueprintPure)
	bool HasRelic2() { return bHasRelic2; }
#pragma endregion
};
