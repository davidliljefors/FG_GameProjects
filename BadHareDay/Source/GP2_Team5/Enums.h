// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"

UENUM(BlueprintType)
enum class ESwapResult : uint8
{
	Success,
	Fail,
	Fail_SameObject,
	Fail_NoRelic1,
	Fail_NoRelic2,
	Fail_OutOfRange,
	Fail_OutOfSight,
	Fail_SameGravity,
};

UENUM(BlueprintType)
enum class EFocusType : uint8
{
	Player	UMETA(DisplayName = "Player"),
	Object	UMETA(DisplayName = "Object"),
	Null	UMETA(DisplayName = "Null"),
};

UENUM(BlueprintType)
enum class ECollectibleType : uint8
{
	Coin,
	Relic1,
	Relic2,
};

UENUM(BlueprintType)
enum class EWinScreen : uint8
{
	Level1Clear,
	Level2Clear,
};

UENUM(BlueprintType)
enum class EOutlineType : uint8
{
	None,
	Hover,
	Selected,
	Swappable,
	NonSwappable,
};

UENUM(BlueprintType)
enum class EAbility : uint8
{
	FirstAbility,
	SecondAbility,
	ThirdAbility,
};

class GP2_TEAM5_API Enums
{
public:
	Enums();
	~Enums();
};
