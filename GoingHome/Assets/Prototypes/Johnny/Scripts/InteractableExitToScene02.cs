using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableExitToScene02 : InteractableBase
{
	protected override void Button1Clicked()
	{
		Debug.Log("Clicked button 1");
		SceneManager.LoadScene("Level_2");
	}

	protected override void Button2Clicked()
	{
		Deselected();
	}

	protected override void Button3Clicked()
	{
		Debug.Log("Clicked button 3");
	}

	protected override void Button4Clicked()
	{
		Deselected();	
	}
}
