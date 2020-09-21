using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    //private Vector3 startPos;
    //public Vector3 endPos;
    //private Vector3 velocity = Vector3.zero;
    //private float smoothTime = 0.2f;
    //bool isSelected;

    private void Start()
    {
        //startPos = transform.position;
        //endPos = transform.position + new Vector3(-20, 0, 0);

    }

    public void Selected()
    {
        //isSelected = true;
        AudioManager.Instance?.Play("ButtonHover");
    }

    public void Deselected()
    {
        //isSelected = false;
    }

    public void ButtonPressed()
    {
        AudioManager.Instance?.Play("ButtonPress");
    }

    /*private void Update()
    {
        if(isSelected)
            transform.position = Vector3.SmoothDamp(transform.position, endPos, ref velocity, smoothTime);
        else
            transform.position = Vector3.SmoothDamp(transform.position, startPos, ref velocity, smoothTime);
    }*/

}
