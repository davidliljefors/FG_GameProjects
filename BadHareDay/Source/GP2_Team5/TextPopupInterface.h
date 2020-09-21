// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Enums.h"
#include "UObject/Interface.h"
#include "TextPopupInterface.generated.h"

// This class does not need to be modified.
UINTERFACE(MinimalAPI)
class UTextPopupInterface : public UInterface
{
	GENERATED_BODY()
};

/**
 * 
 */
class GP2_TEAM5_API ITextPopupInterface
{
	GENERATED_BODY()

	// Add interface functions to this class. This is the class that will be inherited to implement this interface.
public:



	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "C++ Interaction")
	void OnTextPopup(AActor* actor);
	virtual void OnTextPopup_Implementation(AActor* actor);

};
