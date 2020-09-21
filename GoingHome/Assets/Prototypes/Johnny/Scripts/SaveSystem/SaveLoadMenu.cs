using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(AudioSource))]
public class SaveLoadMenu : MonoBehaviour
{
    [SerializeField] SaveEntryUI entryPrefabs;
    [SerializeField] GameObject loadPanel;
    [SerializeField] Transform parent;
    [SerializeField] Button newGame, continueGame, controls, credits, quit;

    [SerializeField] private GameObject fadeScreen;
    [SerializeField] private Animator cutScene;

    private AudioManager audioManager;
    public bool isNewGame;

    void Start()
    {
        audioManager = AudioManager.Instance;
        isNewGame = false;
        newGame.onClick.AddListener(NewGame);
        credits.onClick.AddListener(Credits);
        quit.onClick.AddListener(Quit);

    }

    private void Update()
    {
        if(isNewGame == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                cutScene.SetTrigger("LoadLevel");
        }
    }


    public void NewGame()
    {
        audioManager.FadeOutTheMusic();
        isNewGame = true;
        fadeScreen.SetActive(true);
        cutScene.SetTrigger("FadeNewGame");
    }

    public void ShowLoadMenu()
    {
        loadPanel.SetActive(true);
    }

    public void Credits()
    {
        fadeScreen.SetActive(true);
        cutScene.SetTrigger("FadeCredits");
    }


    public void Quit()
    {
        Debug.Log("Exited the game");
        Application.Quit();

    }

}
