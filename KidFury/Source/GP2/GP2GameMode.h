// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "Kismet/GameplayStatics.h"
#include "Engine/World.h"
#include "GP2GameMode.generated.h"

UCLASS(minimalapi)
class AGP2GameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	AGP2GameMode();

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Currency System")
		int Wallet;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Currency System")
		USoundBase* CollectCoinSound;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Currency System")
		USoundBase* RemoveCoinSound;
	
	UFUNCTION(BlueprintCallable, Category = "Currency System")
		void AddCoinsToWallet(int Amount);
	UFUNCTION(BlueprintCallable, Category = "Currency System")
		void SetWalletToAmount(int Amount);

	UFUNCTION(BlueprintCallable, Category = "Currency System")
		void RemoveCoinsFromWallet(int Amount);

	UFUNCTION(BlueprintCallable, Category = "Currency System")
		int GetCoinsInWallet() { return Wallet; }


};



