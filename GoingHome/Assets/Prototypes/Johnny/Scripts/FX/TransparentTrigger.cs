using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentTrigger : MonoBehaviour
{
    private Transform player;
    private Ray ray;
    private RaycastHit hit;
    private TransparentFX fx;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position, (player.position - transform.position));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                //print(hit.collider.gameObject.name);
            //if hit anything else but player
                if (fx == null)
                {
                    fx = hit.collider.GetComponent<TransparentFX>();

                    if (fx != null)
                    {
                        //run the transparent fx
                        fx.SwitchMaterial(true);
                    }
                }
                else if (hit.collider.CompareTag("Player"))//else if hitting player
                {
                    if (fx != null)
                    {
                        //switch back the transparent fx
                        fx.SwitchMaterial(false);
                        fx = null;
                        
                    }
                }

            }
        }
    }
}
