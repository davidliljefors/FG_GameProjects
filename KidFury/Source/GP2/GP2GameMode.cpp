// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#include "GP2GameMode.h"
#include "GP2PlayerController.h"
#include "GP2Character.h"
#include "UObject/ConstructorHelpers.h"
#include "Kismet/GameplayStatics.h"

AGP2GameMode::AGP2GameMode()
{
	// use our custom PlayerController class
	PlayerControllerClass = AGP2PlayerController::StaticClass();

	// set default pawn class to our Blueprinted character
	//static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/TopDownCPP/Blueprints/TopDownCharacter"));
	//if (PlayerPawnBPClass.Class != NULL)
	//{
	//	DefaultPawnClass = PlayerPawnBPClass.Class;
	//}
}

void AGP2GameMode::AddCoinsToWallet(int Amount)
{
	Wallet += Amount;

	if (CollectCoinSound != nullptr)
	{
		UGameplayStatics::PlaySound2D(GetWorld(), CollectCoinSound);
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("AGP2GameMode::AddCoinsToWallet: No CollectCoinSound set."));
	}
}

void AGP2GameMode::RemoveCoinsFromWallet(int Amount)
{
	if (Wallet >= Amount)
	{
		Wallet -= Amount;
	}
	else
	{
		Wallet = 0;
	}

	if (CollectCoinSound != nullptr)
	{
		UGameplayStatics::PlaySound2D(GetWorld(), RemoveCoinSound);
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("AGP2GameMode::RemoveCoinsFromWallet: No RemoveCoinSound set."));
	}
}

void AGP2GameMode::SetWalletToAmount(int Amount) 
{
	Wallet = Amount;
}
