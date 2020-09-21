using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineOnHover : MonoBehaviour
{
    Color m_MouseOverColor = Color.white;
    Color m_OriginalColor = Color.cyan;
    MeshRenderer m_Renderer;


    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseOver()
    {
        m_Renderer.material.color = m_MouseOverColor;
    }

    private void OnMouseExit()
    {
        m_Renderer.material.color = m_OriginalColor;
    }
}
