using UnityEngine;
using UnityEngine.SceneManagement;

public class ClipHandler : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private string nextScene;

    private StoryManager storyManager;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
    }


    public void ActivateContinueButton()
    {
        continueButton.SetActive(true);
    }

    public void TheEnd()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ContinueToNextClip()
    {
        storyManager.ContinueStory();
    }

    public void FadeOutMusic()
    {
        audioManager.FadeOutTheMusic();
    }

}