//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Utils;
//using UnityEngine.AI;

//public class MiniTesting : MonoBehaviour
//{
//    private MiniGrid grid;
//    Plane plane = new Plane(Vector3.up, Vector3.forward);
//    Ray ray;
//    float distance;
//    Vector3 point;
//    public int x;
//    public float y;
//    public int z;
//    public float cellSize;
//    public int NumberIfClicked;

   
//    private void Awake()
//    {
        
//        grid = new MiniGrid(x, y, z, cellSize, this.transform);
//        //grid = new MiniGrid(4, 2, 20f, this.transform);
       
      

//    }

//    private void Update()
//    {
       
//        if (Input.GetMouseButtonDown(0))
//        {
//            getMousePos();
//        }
//        if(Input.GetMouseButtonDown(1))
//        {
//            Debug.Log(grid.GetValue(getMousePos()));
//        }
//    }

//   public MiniGrid GetGrid
//    {
//        get { return grid; }
//    }

//    private Vector3 getMousePos()
//    {
//        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        if (plane.Raycast(ray, out distance))
//        {
//            point = ray.GetPoint(distance);
//            //Debug.Log(ray.GetPoint(distance));
//            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
//            //go.transform.position = ray.GetPoint(distance);
//            grid.SetValue(point, NumberIfClicked);


//        }
//            return point;
//    }
//}
