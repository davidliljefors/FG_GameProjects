using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteract : MonoBehaviour
{
    [SerializeField] string selectedTag;
    [SerializeField] Actions[] enterActions;
    [SerializeField] Actions[] exitActions;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(selectedTag))
            return;

        for (int i = 0; i < enterActions.Length; i++)
        {
            enterActions[i].Act();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(selectedTag))
            return;

        for (int i = 0; i < exitActions.Length; i++)
        {
            exitActions[i].Act();
        }
    }

}
