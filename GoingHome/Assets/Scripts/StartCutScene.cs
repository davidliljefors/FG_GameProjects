using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCutScene : MonoBehaviour
{
    [SerializeField] private Animator canvasPrefab;
    
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }


    public void LoadNextScene()
    {
        audioManager.FadeOutTheMusic();
    }

    private void OnTriggerEnter(Collider other)
    {
        audioManager.FadeOutTheMusic();
        canvasPrefab.SetTrigger("NextScene");
    }
}
