using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private string nextScene = "Outro";
    private AudioManager audioManager;
    [SerializeField] private Animator creditsAnim;


    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            creditsAnim.SetTrigger("Fade");
    }

    public void ExitCredits()
    {
        Debug.Log("Exiting Scene");
        SceneManager.LoadScene(nextScene);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(nextScene);
        Debug.Log("Next Scene loaded");
    }

    public void FadeOutMusic()
    {
        audioManager.FadeOutTheMusic();
    }

}
