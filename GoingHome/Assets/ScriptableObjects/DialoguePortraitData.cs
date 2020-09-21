using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PortraitData", menuName = "ScriptableObjects/PortraitData", order = 1)]
public class DialoguePortraitData : ScriptableObject
{
	public DialoguePortrait[] portraits;

	public Sprite GetSprite(DialogueSpeaker speaker)
	{
		var result = Array.Find(portraits, x => x.speaker == speaker);
		if (result != null)
		{
			return result.portrait;
		}
		else
		{
			Debug.LogError("No sprite found for DialogueSpeaker." + speaker.ToString());
			return Sprite.Create(Texture2D.whiteTexture, new Rect(Vector2.zero, Vector2.one), Vector2.zero);
		}
	}
}