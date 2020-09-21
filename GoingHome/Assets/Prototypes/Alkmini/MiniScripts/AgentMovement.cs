//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//namespace AI
//{
//	public class AgentMovement : MonoBehaviour
//	{
//		public MiniTesting miniTesting;
//		Vector3 pos = Vector3.zero;
//		MiniGrid grid;
//		float speed;
//		Plane plane = new Plane(Vector3.up, Vector3.forward);
//		Ray ray;
//		float distance;
//		Vector3 m_Point = Vector3.zero;

//		public NavMeshAgent agent;
//		//public Transform target;

//		private void Start()
//		{
//			//agent = GetComponent<NavMeshAgent>();
//			grid = miniTesting.GetGrid;
//		}

//		public void SetDestination(Vector3 destination)
//		{
//			if (grid != null)
//			{
//				agent.SetDestination(GetDestination(destination));
//                StartCoroutine(IsStillWalking());
//            }
//            else
//            {
//                PlayerTurn.CharacterFinished.Invoke();
//            }
//        }

//		private IEnumerator IsStillWalking()
//		{
//			int attempts = 0;
//			while((agent.destination-transform.position).sqrMagnitude > 0.01f && attempts < 50)
//			{
//				attempts++;
//				yield return new WaitForSeconds(0.1f);
//			}

//			PlayerTurn.CharacterFinished.Invoke();
//		}

//        //private void Update()
//        //{
//        //    if (Input.GetMouseButtonDown(0))
//        //    {

//        //        if (grid != null)
//        //        {
//        //            agent.SetDestination(GetDestination(GetMousePos()));
//        //            Debug.Log("path found");
//        //        }
//        //    }

//        //}

//        private Vector3 GetMousePos()
//		{
//			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			if (plane.Raycast(ray, out distance))
//			{
//				m_Point = ray.GetPoint(distance);


//			}
//			return m_Point;

//		}
//		public Vector3 GetDestination(Vector3 point)
//		{
//            for (int i = 0; i < grid.availableTiles.Count - 1; i++)
//            {
//                if (Mathf.Abs(grid.availableTiles[i].x - point.x) < grid.cellSize && Mathf.Abs(grid.availableTiles[i].z - point.z) < grid.cellSize)
//                {
//                    float midX = grid.availableTiles[i].x + (grid.cellSize * 0.5f);
//                    float midZ = grid.availableTiles[i].z + (grid.cellSize * 0.5f);
//                    return new Vector3(midX, point.y, midZ);
//                }
//            }
//            return Vector3.zero;
//		}

//        //private void IsWalkable()
//        //{
//        //    LayerMask mask;

//        //    foreach (Vector3 vec in grid.availableTiles)
//        //    {
//        //        mask = LayerMask.GetMask("Unwalkable");
//        //    }

//        //}
//        //bool TestDirection(int x, int z, int step, int direction)
//        //{

//        //}
//	}
//}
