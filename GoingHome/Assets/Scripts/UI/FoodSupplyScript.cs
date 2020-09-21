using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FoodSupplyScript : MonoBehaviour
{
    [System.NonSerialized] public int currentFood;

    public GameObject foodSpriteHolder;
    public int maxFood;
    private TextMeshProUGUI foodCounter;
    
    private Color m_fadedColor;

    private void Awake()
    {
        GameObject[] foodContainersInScene;
        foodContainersInScene = GameObject.FindGameObjectsWithTag("Interactable");
        maxFood = 0;
        foreach (GameObject container in foodContainersInScene)
        {
            if (container.name.Contains("Food"))
            {
                maxFood++;
                Debug.Log(container.name);
            }
        }
    }
    private void Start()
    {
        currentFood = 0;
        foodCounter = foodSpriteHolder.GetComponentInChildren<TextMeshProUGUI>();
        m_fadedColor = new Color(0, 0, 0, 0.5f);
        UpdateInventoryUi();




    }

    public void AddFood(int amountAdded)
    {
        //Adds food if there is space
        if (currentFood < maxFood)
        {
            currentFood += amountAdded;
            UpdateInventoryUi();
        }
    }
    public void RemoveFood(int amountRemoved)
    {
        currentFood -= amountRemoved;
        //PlayerPrefs.SetInt("StoredFood", currentFood);
        UpdateInventoryUi();
    }

    private void UpdateInventoryUi()
    {
        ////Updates the UI elements
        foodSpriteHolder.transform.GetChild(1).GetComponent<Animation>().Play();
        foodCounter.text = currentFood.ToString() + "/" + maxFood;


        //foodText.text = "Food: " + currentFood.ToString() + "/" + maxFood.ToString();
        //float foodBarValue = (float)currentFood / (float)maxFood;

        //foodBar.GetComponent<Image>().fillAmount = foodBarValue;

        //for (int i = 0; i < maxFood; i++)
        //{
        //    if(i < currentFood)
        //    {
        //        foodSpriteHolder.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        //        foodSpriteHolder.transform.GetChild(i).GetComponent<Animation>().Play();

        //    }
        //    else
        //    {
        //        foodSpriteHolder.transform.GetChild(i).GetComponent<Image>().color = m_fadedColor;
        //    }
            
        //}

    }

//#if UNITY_EDITOR
//    //RESETS STORED FOOD WHEN EXITS PLAY MODE. 
//    //
//    //Does only apply in editor, not in build.
//    [InitializeOnLoad]
//    public static class PlayStateNotifier
//    {

//        static PlayStateNotifier()
//        {
//            EditorApplication.playModeStateChanged += ModeChanged;
//        }

//        static void ModeChanged(PlayModeStateChange playModeState)
//        {
//            if (playModeState == PlayModeStateChange.EnteredEditMode)
//            {
//                //Sets "Stored Food" to 0 on quitting playmode
//                //PlayerPrefs.SetInt("StoredFood", 0);
//            }
//        }
//    }
//#endif
}
