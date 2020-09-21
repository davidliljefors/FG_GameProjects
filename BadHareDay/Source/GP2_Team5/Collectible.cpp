// Fill out your copyright notice in the Description page of Project Settings.

#include "Collectible.h"

#include "GameFramework/RotatingMovementComponent.h"
#include "Components/SphereComponent.h"
#include "GravityCharacter.h"
#include "Components/StaticMeshComponent.h"

// Sets default values
ACollectible::ACollectible()
{
	PrimaryActorTick.bCanEverTick = true;

	SphereCollision = CreateDefaultSubobject<USphereComponent>(TEXT("Sphere Collision"));
	SphereCollision->SetCollisionProfileName(FName("OverlapAllDynamic"));
	RootComponent = SphereCollision;

	Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Mesh"));
	Mesh->SetupAttachment(RootComponent);
	Mesh->SetCollisionProfileName(FName("NoCollision"));

	RotatingMovementCmp = CreateDefaultSubobject<URotatingMovementComponent>(TEXT("Rotating Movement"));
}

void ACollectible::BeginPlay()
{
	Super::BeginPlay();

	SphereCollision->OnComponentBeginOverlap.AddDynamic(this, &ACollectible::OnComponentBeginOverlap);
}

// Called every frame
void ACollectible::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}
										   
void ACollectible::OnComponentBeginOverlap(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	UE_LOG(LogTemp, Warning, L"picked up coin");
	if (OtherActor->GetClass()->IsChildOf(AGravityCharacter::StaticClass()))
	{
		AGravityCharacter* Player = Cast<AGravityCharacter>(OtherActor);
		Player->AddCollectible(this);
		OnPickedUp(Player);
	}
}

