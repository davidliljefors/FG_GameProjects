//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GridGeneration : MonoBehaviour
//{
//    Vector3 gridStart;
//    MeshFilter _mesh;
//    Vector3[] test;

//    private void Awake()
//    {
//    }
//    public Vector3 myPos;
//    private void OnDrawGizmos()
//    {
//        _mesh = gameObject.GetComponent<MeshFilter>();
//        test = _mesh.mesh.vertices;
//        Vector3[] test0 = test;
        
//        Gizmos.color = Color.red;
//        Vector3 gridLineEnd = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
//        Vector3 gridLineStart = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
//        Gizmos.DrawLine(gridLineStart, gridLineEnd);
//        Gizmos.color = Color.cyan;

        
//        for (int i = 0; i < test0.Length; i++)
//        {
           
//            Vector3 startLine = transform.TransformPoint(test0[i]);
//            Vector3 endLine = transform.TransformPoint(test0[i + 1]);
//            Gizmos.DrawLine(startLine, endLine);
//        }
        


//    }
//}
