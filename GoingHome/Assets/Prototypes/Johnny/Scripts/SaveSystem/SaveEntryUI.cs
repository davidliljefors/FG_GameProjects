using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveEntryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sceneDesc, date;
    [SerializeField] Button loadButton;
    [SerializeField] RawImage screenCaptured;

    private int id;

    public void InitLoadEntry(Texture2D ss, string sceneName, string date, int id)
    {
        sceneDesc.text = sceneName;
        this.date.text = date;

        this.id = id;
        screenCaptured.texture = ss;
        loadButton.onClick.AddListener(LoadData);
    }

    void LoadData()
    {
        //we are going to implement loading later
        //DataManager.Instance.LoadDataEntry(id);
    }
}
