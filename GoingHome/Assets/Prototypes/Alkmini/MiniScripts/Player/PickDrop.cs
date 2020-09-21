using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDrop : MonoBehaviour
{
    //PickUp
    public GameObject attachedItem;

    private void OnTriggerEnter(Collider item)
    {
        if (item.CompareTag("Item"))
        {
            item.transform.parent = gameObject.transform;
            attachedItem = item.gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.U))
        {
            attachedItem.transform.parent = null;
        }
    }




}
