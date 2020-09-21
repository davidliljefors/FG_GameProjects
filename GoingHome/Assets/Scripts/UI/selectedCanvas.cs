using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedCanvas : MonoBehaviour
{
    private Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(m_camera.transform.forward);
    }
}
