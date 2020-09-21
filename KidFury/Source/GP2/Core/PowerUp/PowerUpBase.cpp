// Fill out your copyright notice in the Description page of Project Settings.


#include "PowerUpBase.h"
#include "GP2\GP2Character.h"

// Sets default values for this component's properties
UPowerUpBase::UPowerUpBase()
{

}


// Called when the game starts
void UPowerUpBase::BeginPlay()
{
	Super::BeginPlay();

	// ...

}

void UPowerUpBase::ExecuteDash(AActor* Owner, float DashForce)
{
	if (Owner->IsA<AGP2Character>())
	{
		Owner->SetActorLocation(Owner->GetActorForwardVector() * DashForce, true);

		/*Character->SetSimulatePhysics(true);
		Character->AddImpulse(FVector(0.0f, 0.0f, DashForce));
		Character->AddImpulse(FVector(0.0f, 0.0f, DashForce) * Character->GetMass());*/

		//TODO: Execute the Dash move
		UE_LOG(LogTemp, Warning, TEXT("UPowerUpBase::ExecuteDash | TODO: Create dash movement in cpp file..."));
	}
}



