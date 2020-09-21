//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GridMovement : MonoBehaviour
//{
//    [SerializeField] GridGenerator gridGenerator;
//    Vector3 up = Vector3.zero,
//    right = new Vector3(0, 90, 0),
//    down = new Vector3(0, 180, 0),
//    left = new Vector3(0, 270, 0),
//    currentDirection = Vector3.zero;

//    Vector3 nextPos, destination, direction;

//    float speed = 5f;
//    float rayLength = 1f;

//    bool canMove;

//    void Start()
//    {
//        currentDirection = up;
//        nextPos = Vector3.forward;
//        destination = transform.position;
               
//    }

//    void Update()
//    {
//        Move();

//    }

//    // I call this from PlayerInput to set destination when you press ground after selecting a player
//    public void SetDestination(Vector3 newDestination)
//    {
//        destination = newDestination + Vector3.up;
//        if (gridGenerator.points.Contains(CalculateClosestMinimum(destination)) && gridGenerator.points.Contains(CalculateClosestMaximum(destination)))
//		{
//			destination = (CalculateClosestMaximum(destination) + (CalculateClosestMinimum(destination) - CalculateClosestMaximum(destination)) / 2) + Vector3.up * transform.position.y;
//		}
//	}

//    private void Move()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

//        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            nextPos = Vector3.forward;
//            currentDirection = up;
//            canMove = true;
//        }
//        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            nextPos = Vector3.back;
//            currentDirection = down;
//            canMove = true;
//        }
//        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            nextPos = Vector3.right;
//            currentDirection = right;
//            canMove = true;
//        }
//        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            nextPos = Vector3.left;
//            currentDirection = left;
//            canMove = true;

//        }

//        //if (Vector3.Distance(destination, transform.position) <= 0.000001f)
//        //{
//        //    transform.localEulerAngles = currentDirection;
//        //    if (canMove)
//        //    {
//        //        if (Valid() == true)
//        //        {
//        //            destination = transform.position + nextPos;
//        //            direction = nextPos;
//        //            canMove = false;

//        //        }

//        //    }
//        //}


//        //if (Input.GetMouseButtonDown(0))
//        //{
//        //    RaycastHit hitInfo;
//        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        //    if (Physics.Raycast(ray, out hitInfo))
//        //    {
//        //        destination = hitInfo.point;
//        //        transform.position = destination;
//        //        MoveOnGrid(hitInfo.point);
//        //    }

//        //}


//        // I commented this -David, input comes from PlayerInput instead
//        //if (Input.GetMouseButtonDown(0))
//        //{
//        //    RaycastHit hitInfo;
//        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        //    if (Physics.Raycast(ray, out hitInfo))
//        //    {
//        //        destination = hitInfo.point + Vector3.up;
                
//        //        //transform.position = destination;
//        //        if (gridGenerator.points.Contains(CalculateClosestMinimum(destination)) && gridGenerator.points.Contains(CalculateClosestMaximum(destination)))
//        //        {
//        //           destination = (CalculateClosestMaximum(destination) + (CalculateClosestMinimum(destination) - CalculateClosestMaximum(destination))/2);
//        //        }
//        //    }

//        //}

//    }
//    Vector3 CalculateClosestMaximum(Vector3 point)
//    {
//        float pointX = Mathf.CeilToInt(point.x) - 0.5f;
//        float pointZ = Mathf.CeilToInt(point.z) - 0.5f;
//        Vector3 largestPoint = new Vector3(pointX, 0, pointZ);
//        return largestPoint;

//    }

//    Vector3 CalculateClosestMinimum(Vector3 point)
//    {
//        float pointX = Mathf.FloorToInt(point.x) - 0.5f;
//        float pointZ = Mathf.FloorToInt(point.z) - 0.5f;
//        Vector3 smallestPoint = new Vector3(pointX, 0, pointZ);
//        return smallestPoint;

//    }
//    void MoveOnGrid(Vector3 clickPoint)
//    {
//        var finalPosition = gridGenerator.GetNearestPointOnGrid(clickPoint);
//    }


//    bool Valid()
//    {
//        Ray ray = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
//        RaycastHit hit;

//        Debug.DrawRay(ray.origin, ray.direction, Color.red);

//        if (Physics.Raycast(ray, out hit, rayLength))
//        {
//            if (hit.collider.tag == "Tree")
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//}
