using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMap : MonoBehaviour
{
    private Transform m_characterParent;
    private int m_foodToRemove;
    private bool m_canLeaveScene = false;
    public GameObject notEnoughFoodGo;
    private FoodSupplyScript foodSupply;

    private AudioManager audioManager;
    [SerializeField] private Animator worldMapAnim;
    [SerializeField] private string loadLevel;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        m_characterParent = GameObject.Find("PlayerCharacters").transform;
        notEnoughFoodGo.SetActive(false);
        foodSupply = FindObjectOfType<FoodSupplyScript>();
    }

    private void RemoveFood()
    {
        m_foodToRemove = 0;
        //m_foodToRemove = m_characterParent.childCount;
        for (int i = 0; i < m_characterParent.childCount; i++)
        {
            if(m_characterParent.GetChild(i).gameObject.activeInHierarchy)
            {
                m_foodToRemove++;
            }
        }
        if (m_foodToRemove <= foodSupply.currentFood)
        {
            //m_characterParent.GetComponent<FoodSupplyScript>().RemoveFood(m_foodToRemove);
            m_canLeaveScene = true;
        }

        else
        {
            Debug.Log("Not enough food to leave! You have " + PlayerPrefs.GetInt("StoredFood") + " but it costs " + m_foodToRemove);
            notEnoughFoodGo.SetActive(true);
            m_canLeaveScene = false;
        }
    }

    public void CloseWorldMap()
    {
        gameObject.SetActive(false);
    }

    public void FadeOutMusic()
    {
        audioManager.FadeOutTheMusic();
    }

    public void StartFade()
    {
        worldMapAnim.SetTrigger("Fade");
    }
    
    public void LoadNewLevel()
    {
        if (Application.CanStreamedLevelBeLoaded(loadLevel))
        {
            RemoveFood();
            if (m_canLeaveScene)
            {
                SceneManager.LoadScene(loadLevel);
                m_characterParent.GetComponent<FoodSupplyScript>().RemoveFood(m_foodToRemove);
            }
            
        }
        else
        {
            Debug.Log("Scene does not exist... Yet.");
            
        }
    }
}
