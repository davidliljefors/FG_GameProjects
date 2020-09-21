using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentFX : MonoBehaviour
{
    [SerializeField] Material transparentMaterial;

    Material[] originals;
    MeshRenderer[] meshRenderers;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originals = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originals[i] = meshRenderers[i].material;
        }
    }

    public void SwitchMaterial(bool transparent)
    {
        if (transparent)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = transparentMaterial;
            }
        }
        else
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = originals[i];
            }
        }
    }
}
