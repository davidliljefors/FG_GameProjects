using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    private Canvas myCanvas;



    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        myCanvas = GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.WorldSpace;
        //myCanvas.transform.localPosition = new Vector3(0, 1.5f, 0);
        myCanvas.transform.localScale = new Vector3(0.02f, 0.015f, 0.015f);
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }

}

