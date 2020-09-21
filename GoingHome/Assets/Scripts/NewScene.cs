using UnityEngine.SceneManagement;
using UnityEngine;

public class NewScene : MonoBehaviour
{
    public string nextScene;

    public void LoadNewScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
