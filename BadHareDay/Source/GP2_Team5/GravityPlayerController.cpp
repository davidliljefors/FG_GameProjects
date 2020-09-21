// Fill out your copyright notice in the Description page of Project Settings.


#include "GravityPlayerController.h"
#include "Blueprint/UserWidget.h"

void AGravityPlayerController::BeginPlay()
{
	Super::BeginPlay();
}

void AGravityPlayerController::ClearLevel(EWinScreen ClearScreen)
{
	UUserWidget* Screen = nullptr;
	switch (ClearScreen)
	{
	case EWinScreen::Level1Clear:
		Screen = CreateWidget(this, Level1ClearScreenClass);
		break;
	case EWinScreen::Level2Clear:
		Screen = CreateWidget(this, Level2ClearScreenClass);
		break;
	default:
		break;
	}

	if (Screen != nullptr)
	{
		Screen->AddToViewport();
	}
}

