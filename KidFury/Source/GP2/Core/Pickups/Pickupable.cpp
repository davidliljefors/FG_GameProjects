// Fill out your copyright notice in the Description page of Project Settings.


#include "Pickupable.h"
#include <GP2\GP2GameMode.h>
#include "Math/UnrealMathUtility.h"

// Sets default values
APickupable::APickupable()
{
	CollisionComponent = CreateDefaultSubobject<USphereComponent>(TEXT("CollisionComponent"));
	CollisionComponent->BodyInstance.SetCollisionProfileName(TEXT("Pickupable"));
	CollisionComponent->BodyInstance.bLockXRotation = true;
	CollisionComponent->BodyInstance.bLockYRotation = true;
	CollisionComponent->SetCollisionResponseToAllChannels(ECollisionResponse::ECR_Ignore);
	CollisionComponent->SetCollisionResponseToChannel(ECollisionChannel::ECC_Pawn, ECollisionResponse::ECR_Overlap);
	CollisionComponent->SetCollisionResponseToChannel(ECollisionChannel::ECC_WorldStatic, ECollisionResponse::ECR_Block);

	StaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("StaticMeshComponent"));
	StaticMeshComponent->SetupAttachment(CollisionComponent);
	StaticMeshComponent->SetCollisionEnabled(ECollisionEnabled::NoCollision);
	StaticMeshComponent->CanCharacterStepUp(false);

	RotatingMovementComponent = CreateDefaultSubobject<URotatingMovementComponent>(TEXT("RotatingMovementComponent"));

	RootComponent = CollisionComponent;
}

// Called when the game starts or when spawned
void APickupable::BeginPlay()
{
	Super::BeginPlay();
	RotatingMovementComponent->RotationRate = FRotator(0.0f, RotateSpeed, 0.0f);
	TossObjectIntoAir(bTossIntoAir);
}

/**launch object upwards in a random direction in a 360 degree radius*/
void APickupable::TossObjectIntoAir(bool TossIntoAir)
{
	if (TossIntoAir)
	{
		UPrimitiveComponent* Pickup = Cast<UPrimitiveComponent>(RootComponent);
		Pickup->SetCollisionEnabled(ECollisionEnabled::QueryAndPhysics);
		Pickup->SetSimulatePhysics(true);

		float Angle = FMath::RandRange(0.0f, 360.0f);
		Pickup->AddImpulse(FVector(FMath::Cos(Angle) * 100, FMath::Sin(Angle) * 100, 800.0f) * Pickup->GetMass());
	}
}

