using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcusTestCamera : MonoBehaviour
{
    public float mouseSensitivity = 4f;
    public Transform target;
    public float dstFromTarget = 60f;
    public Vector2 pitchMinMax = new Vector2(15,40);

    public float rotationSmoothTime = .2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    public bool rightMouse;

    private void Update() {
        if (Input.GetMouseButton(2)){
            rightMouse = true;
            MoveCamera();
        }
    }

    void MoveCamera() {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * -mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
