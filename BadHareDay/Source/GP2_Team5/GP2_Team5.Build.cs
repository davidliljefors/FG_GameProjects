// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class GP2_Team5 : ModuleRules
{
	public GP2_Team5(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { 
			"Core", 
			"CoreUObject", 
			"Engine", 
			"InputCore", 
			"HeadMountedDisplay", 
			"UMG", 
			"AIModule",
			"ApexDestruction",
		});
	}
}
