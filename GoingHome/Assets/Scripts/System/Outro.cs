using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    [SerializeField] private Animator ScreenFade;
    [SerializeField] private string loadScene;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            ScreenFade.SetTrigger("FadeOut");
            Debug.Log("To Main Menu");
        }
    }

    public void FadeOutMusic()
    {
        audioManager.FadeOutTheMusic();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(loadScene);
    }

}
