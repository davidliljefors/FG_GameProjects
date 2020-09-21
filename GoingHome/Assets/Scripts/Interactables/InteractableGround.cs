using UnityEngine;

public class InteractableGround : InteractableBase
{

	private void Start()
	{
		Debug.Log("start");

	}

	protected override void Button1Clicked()
	{
		Debug.Log("Walk to pos");
	}

	protected override void Button2Clicked()
	{
		Deselected();
	}
}
