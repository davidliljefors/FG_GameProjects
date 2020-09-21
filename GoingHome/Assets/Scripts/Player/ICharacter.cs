using System;
using UnityEngine;

/// <summary>
/// Interface for characters
/// </summary>
public interface ICharacter
{
	Item CarriedItem { get; set; }
	void PerformInteraction(Action action, int cost);
}
