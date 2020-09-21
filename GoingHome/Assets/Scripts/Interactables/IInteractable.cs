using UnityEngine;

/// <summary>
/// Interface for interactable game objects in the world
/// </summary>
public interface IInteractable
{
	bool InRange { get; set; }
	void Clicked(GameObject clickedBy);
	void Deselected();
}
