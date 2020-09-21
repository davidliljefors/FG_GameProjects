using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    [SerializeField] private Animator ScreenFade;


    void Start()
    {
        ScreenFade.SetTrigger("FadeIn");
    }

    public void ToggleScreen()
    {
        
    }
}
