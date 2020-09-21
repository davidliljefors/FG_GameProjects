using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool m_gamePaused = false;
    private GameObject m_MenuGo;

    public bool canSlideOut;
    public bool canSlideIn;

    [SerializeField] private Animator pauseMenuAnim;

    IEnumerator SlideOutPauseMenu (float transitionTime)
    {
        pauseMenuAnim.SetTrigger("SlideOut");
        yield return new WaitForSeconds(transitionTime);
        m_MenuGo.SetActive(false);
        canSlideIn = true;
    }

    IEnumerator SlideInPauseMenu(float transitionTime)
    {
        m_MenuGo.SetActive(true);
        pauseMenuAnim.SetTrigger("SlideIn");
        yield return new WaitForSeconds(transitionTime);
        canSlideOut = true;
    }

    private void Start()
    {
        canSlideOut = false;
        canSlideIn = true;
        m_MenuGo = transform.GetChild(0).gameObject;
        m_MenuGo.SetActive(false);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    //Acually does not pause game, but simply disabling click-input by blocking it with an image
    public void PauseGame()
    {

        if (canSlideIn)
        {
            m_gamePaused = true;
            canSlideIn = false;
            StartCoroutine(SlideInPauseMenu(1f));
        }
    }

    public void ResumeGame()
    {

        if (canSlideOut)
        {
            m_gamePaused = false;
            canSlideOut = false;
            StartCoroutine(SlideOutPauseMenu(0.4f));
        }


    }
    public void SaveGame()
    {
        Debug.Log("Nope. No saving.");
    }

    public void LoadGame()
    {
        Debug.Log("Oh you tried this too. No saving = no loading");
    }

    public void ToMenu()
    {
        Debug.Log("Add main menu scene in script here when that scene is available");
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game will quit in build");
    }

    public void MenuSoundOpen()
    {
        AudioManager.Instance?.Play("OpenMenu");
    }


}
