using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetTest : MonoBehaviour
{

    public Transform levelTarget;       //target in middle of level, could be replaced with Vector3.zero i guess
    public Transform characterTarget;   //character target

    public float dstFromTarget = 42f;
    public int divider = 9;

    public float positionSmoothTime = .1f;
    Vector3 positionSmoothVelocity;
    Vector3 currentPosition;

    Vector3 lookTarget;
    Vector3 nextPosition;

    private void Start() {

        currentPosition = transform.position;   //prevents camera from starting at 0,0,0 which looks crazy because of motion blur
    }

    private void Update() {

        UpdateTarget();
        MoveCamera();

    }

    void UpdateTarget() {
        lookTarget = ((levelTarget.position + characterTarget.position) / divider);     //put actual look target in between the level target and character target. divider weights position either way. 
                                                                                        //divide by 2 to go in the middle, higher to go more towards level target (less movement)
    }

    void MoveCamera() {

        nextPosition = lookTarget - transform.forward * dstFromTarget;

        currentPosition = Vector3.SmoothDamp(currentPosition, nextPosition, ref positionSmoothVelocity, positionSmoothTime); //lerp smoothy magic

        transform.position = currentPosition;

    }
}
