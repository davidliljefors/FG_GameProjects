using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAnimator : MonoBehaviour
{

    public void TurnOffCamAnim()
    {
        GetComponent<Animator>().enabled = false;
    }


}
