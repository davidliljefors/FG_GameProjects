//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CubePlacer : MonoBehaviour
//{
//    private GridGenerator gridGenerator;

//    private void Awake()
//    {
//        gridGenerator = FindObjectOfType<GridGenerator>();
//    }

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            RaycastHit hitInfo;
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out hitInfo))
//            {
//                PlaceCubeNear(hitInfo.point);
//            }
//        }
//    }

//    private void PlaceCubeNear(Vector3 clickPoint)
//    {
//        var finalPosition = gridGenerator.GetNearestPointOnGrid(clickPoint);
//        GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

//        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = nearPoint;
//    }
//}

