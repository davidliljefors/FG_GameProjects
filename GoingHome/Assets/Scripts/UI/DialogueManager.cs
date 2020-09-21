using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DialoguePortrait
{
	public DialogueSpeaker speaker;
	public Sprite portrait;
}

public enum DialogueSpeaker
{
	Mickey,
	Tickie,
	Stitchy,
	Lizard
}

[System.Serializable]
public struct DialogueData
{
	public DialogueSpeaker speaker;
	[TextArea]
	public string text;
	public Actions[] actions;
	//public GameObject[] diaGO;
	//public bool activeStatus;
	//public GameObject dialogueEventObject;
	//[SerializeField] Actions[] actions;
	//[SerializeField] AudioClip[] audioClips;
	//[SerializeField] bool isMusic;
}

//public class CustomGameObject
//{
//	[SerializeField] GameObject gO;
//	[SerializeField] bool activeStatus;

//	public GameObject GO { get { return dialogueGO; } }
//	public bool ActiveStatus { get { return activeStatus; } }
//}

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private float typingSpeed = 0.02f;
	public GameObject playerHUD;

	[Header("Dialogue Speech Bubbles")]
	[SerializeField] private GameObject speechBubble;

	[Header("Dialogue TMP text")]
	[SerializeField] private TextMeshProUGUI dialogueText;

	[Header("Continue Buttons")]
	[SerializeField] private GameObject continueButton;

	[Header("Dialogue Animations")]
	[SerializeField] private Animator animator;

	[Header("Dialogue Portrait Image object")]
	[SerializeField] private Image portrait;

	[SerializeField] private DialoguePortraitData portraitData;
	[SerializeField] private DialogueData[] dialogueData;

	private float fadeTime = 0.3f;
	private int currentDialogueIndex = -1;
	[SerializeField] public bool dialogueOver;

	[SerializeField] Actions[] actions;
	//[SerializeField] private GameObject diaGO;

	//private AudioClip[] audioClips;
	//private bool isMusic;

	//private AudioManager manager;
	//[SerializeField] Actions[] actions;

	//[System.Serializable]
	//public class CustomGameObject
	//{
	//	[SerializeField] customGameObjects gO;
	//	[SerializeField] bool activeStatus;

	//	//public GameObject GO { get { return gO; } }
	//	//public bool ActiveStatus { get { return activeStatus; } }
	//}

	//private void OnEnable()
	//{
	//	Extensions.RunActions(actions);
	//}

	void Start()
	{
		StartCoroutine(StartDialogueAfterTime(1f));
	}

	IEnumerator StartDialogueAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		StartDialogue();
	}

	private void OnEnable()
	{
		Extensions.RunActions(actions);
	}

	public void StartDialogue()
	{
		currentDialogueIndex = 0;
		dialogueOver = false;


		StartCoroutine(StartFadeIn());
	}

	private IEnumerator StartFadeIn()
	{
		speechBubble.SetActive(true);
		portrait.sprite = portraitData.GetSprite(dialogueData[currentDialogueIndex].speaker);
		animator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(fadeTime);
		StartCoroutine(TypePlayerDialogue());
	}

	private IEnumerator FadeToDialogue()
	{
		animator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(fadeTime);
		portrait.sprite = portraitData.GetSprite(dialogueData[currentDialogueIndex].speaker);
		animator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(fadeTime);
		StartCoroutine(TypePlayerDialogue());
	}

	IEnumerator TypePlayerDialogue()
	{
		// Play dialogue audio
		//Extensions.RunActions(actions);

		bool escapeCharacterSeen = false;
		bool skipWriting = false;

		// Write speaker name and then newline
		dialogueText.text += "<color=\"white\">" + dialogueData[currentDialogueIndex].speaker.ToString() + ": </color> \n"; 

		foreach (char letter in dialogueData[currentDialogueIndex].text.ToCharArray())
		{
			if(Input.GetMouseButton(0))
			{
				skipWriting = true;
			}

			if (letter == '<')
			{
				escapeCharacterSeen = true;
			}
			if (letter == '>')
			{
				escapeCharacterSeen = false;
			}

			dialogueText.text += letter;

			if (!escapeCharacterSeen && !skipWriting)
			{
				yield return new WaitForSeconds(typingSpeed);
			}
		}
		continueButton.SetActive(true);
	}

	public void ContinueDialogue()
	{
		continueButton.SetActive(false);
		DialogueSpeaker lastDialogue = dialogueData[currentDialogueIndex].speaker;
		currentDialogueIndex++;

		if (currentDialogueIndex < dialogueData.Length)
		{
			if (dialogueData[currentDialogueIndex].speaker == lastDialogue)
			{
				dialogueText.text = "";
				StartCoroutine(TypePlayerDialogue());

			}
			else
			{
				dialogueText.text = "";
				StartCoroutine(FadeToDialogue());
			}
		}
		else
		{
			Debug.Log("Dialogue over!");

			StartCoroutine(FadeAndEndDialogue());

			dialogueOver = true;
		}
	}

	private IEnumerator FadeAndEndDialogue()
	{
		animator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(fadeTime);
		speechBubble.SetActive(false);
		playerHUD.SetActive(true);
	}
}
