using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateActions : Actions
{
    [SerializeField] List<CustomGameObject> customGameObjects = new List<CustomGameObject>();

    public override void Act()
    {
        for (int i = 0; i < customGameObjects.Count; i++)
        {
            customGameObjects[i].GO.SetActive(customGameObjects[i].ActiveStatus);
        }
    }
}

[System.Serializable]
public class CustomGameObject
{
    [SerializeField] GameObject gO;
    [SerializeField] bool activeStatus;

    public GameObject GO { get { return gO; } }
    public bool ActiveStatus { get { return activeStatus; } }
}
