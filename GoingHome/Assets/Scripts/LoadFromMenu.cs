using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadFromMenu : MonoBehaviour
{
    public string newGame;
    public string credits;
    public SaveLoadMenu saveLoadMenu;
    [SerializeField] private Animator fadeAnimator;

    public void LoadNewScene()
    {
        PlayerPrefs.SetInt("StoredFood", 0);
        SceneManager.LoadScene(newGame);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(credits);
    }

    IEnumerator WaitBeforeFadeText(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Timer ended!");
        fadeAnimator.SetTrigger("FirstText");
    }

    public void FadeInFirstText()
    {
        StartCoroutine(WaitBeforeFadeText(1f));
    }

    public void FadeInSecondText()
    {
        fadeAnimator.SetTrigger("SecondText");
    }

    public void FadeInThirdText()
    {
        fadeAnimator.SetTrigger("ThirdText");
    }

    public void FadeOutText()
    {
        fadeAnimator.SetTrigger("FadeOut");
    }

}
