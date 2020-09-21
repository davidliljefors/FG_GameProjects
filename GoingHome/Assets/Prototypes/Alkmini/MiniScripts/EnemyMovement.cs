//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using AI;

//public class EnemyMovement : MonoBehaviour
//{
//    MiniGrid grid;
//    MiniTesting miniTesting;
//    private AgentMovement agent;
//    public NavMeshAgent enemyAgent;
//    bool hasReached = true;
//    Vector3 newPos;
//    public Transform target;

//    private void Awake()
//    {

//        enemyAgent = enemyAgent.GetComponent<NavMeshAgent>();
        
//        agent = FindObjectOfType<AgentMovement>();
//        miniTesting = FindObjectOfType<MiniTesting>();
//        if (miniTesting != null)
//        {
//            grid = miniTesting.GetGrid;

//        }
//    }

//    private void Update()
//    {
//        enemyAgent.SetDestination(target.transform.position);
////         if (enemyAgent.isStopped)
////         {
////             enemyAgent.SetDestination(new Vector2(enemyAgent.transform.position.x, agent.transform.position.z));
////         }
//    }
//}
