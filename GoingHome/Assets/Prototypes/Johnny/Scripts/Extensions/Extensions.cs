using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public static class Extensions
{
    //public static bool IsMouseOverUI()
    //{
    //    return EventSystem.current.IsPointerOverGameObject();
    //}
        
    public static void RunActions(Actions[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act();
        }
    }

    //public static List<T> FindObjectsOfTypeAll<T>()
    //{
    //    List<T> result = new List<T>();
    //    SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => result.AddRange(g.GetComponentsInChildren<T>()));

    //    return result;
    //}
        
}
