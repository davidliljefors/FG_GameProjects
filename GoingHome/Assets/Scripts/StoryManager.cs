using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    private enum Shot
    {
        Shot01,
        Shot02,
        Shot03,
        Shot04,
        Shot05,
        Shot06,
        End

    }

    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI storyText = null;
    [SerializeField] private GameObject continueButton = null;
    [SerializeField] private Animator cutSceneAnim = null;
    [SerializeField] private string nextScene = null;

    [TextArea, Space]
    [SerializeField] private string[] storySentences;
    [SerializeField] private Shot[] shotOrder;

    private int storyIndex;
    private int currentShotIndex = -1;
    public bool continueButtonIsActivated;
    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.Instance;
        continueButtonIsActivated = false;
        currentShotIndex = 0;
        StartCoroutine(TypeStoryText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndStory();
        }

    }

    IEnumerator TypeStoryText()
    {
        foreach (char letter in storySentences[storyIndex].ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            
            if (continueButtonIsActivated == true)
            {
                continueButton.SetActive(true);
                Debug.Log("lossssssl");
            }
        }
        storyIndex++;

            
        
    }

    public void ContinueStory()
    {
        Shot lastShot = shotOrder[currentShotIndex];
        continueButton.SetActive(false);
        storyText.text = "";
        currentShotIndex++;

        if (currentShotIndex < shotOrder.Length)
        {
            if (shotOrder[currentShotIndex] == Shot.Shot01)
            {
                    Debug.Log("Continue Shot 01");
                    StartCoroutine(TypeStoryText());
            }

            else if (shotOrder[currentShotIndex] == Shot.Shot02)
            {
                if (lastShot == Shot.Shot02)
                {
                    continueButtonIsActivated = true;
                    StartCoroutine(TypeStoryText());
                }
                else
                {
                    StartCoroutine(TypeStoryText());
                    Debug.Log("Fade to Shot 02");
                    cutSceneAnim.SetTrigger("Shot02");
                }

            }

            else if (shotOrder[currentShotIndex] == Shot.Shot03)
            {
                if (lastShot == Shot.Shot03)
                {
                    continueButtonIsActivated = true;
                    StartCoroutine(TypeStoryText());
                }
                else
                {
                    StartCoroutine(TypeStoryText());
                    Debug.Log("Fade to Shot 03");
                    cutSceneAnim.SetTrigger("Shot03");
                }

            }

            else if (shotOrder[currentShotIndex] == Shot.Shot04)
            {
                if (lastShot == Shot.Shot04)
                {
                    continueButtonIsActivated = true;
                    StartCoroutine(TypeStoryText());
                }
                else
                {
                    StartCoroutine(TypeStoryText());
                    Debug.Log("Fade to Shot 04");
                    cutSceneAnim.SetTrigger("Shot04");
                }

            }

            else if (shotOrder[currentShotIndex] == Shot.Shot05)
            {
                if (lastShot == Shot.Shot05)
                {
                    continueButtonIsActivated = true;
                    StartCoroutine(TypeStoryText());
                }
                else
                {
                    StartCoroutine(TypeStoryText());
                    Debug.Log("Fade to Shot 05");
                    cutSceneAnim.SetTrigger("Shot05");
                }

            }

            else if (shotOrder[currentShotIndex] == Shot.Shot06)
            {
                if (lastShot == Shot.Shot06)
                {
                    continueButtonIsActivated = true;
                    StartCoroutine(TypeStoryText());
                }
                else
                {
                    StartCoroutine(TypeStoryText());
                    Debug.Log("Fade to Shot 06");
                    cutSceneAnim.SetTrigger("Shot06");
                }

            }

            else if (shotOrder[currentShotIndex] == Shot.End)
            {
                audioManager.FadeOutTheMusic();
                EndStory();
            }

        }
    }

    public void EndStory()
    {
        Debug.Log("Story time is over!");
        cutSceneAnim.SetTrigger("End");
    }


}