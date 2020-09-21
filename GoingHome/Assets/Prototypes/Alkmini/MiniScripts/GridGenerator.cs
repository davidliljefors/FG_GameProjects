//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class GridGenerator : MonoBehaviour
//{
//    public List<Vector3> points = new List<Vector3>();
//    [SerializeField] public int m_GridSize = 0;
//    private float size = 1f;


//    public Vector3 GetNearestPointOnGrid(Vector3 position)
//    {
//        position -= transform.position;

//        int xCount = Mathf.RoundToInt(position.x / size);
//        int yCount = Mathf.RoundToInt(position.y / size);
//        int zCount = Mathf.RoundToInt(position.z / size);

//        Vector3 result = new Vector3(
//            (float)xCount * size,
//            (float)yCount * size,
//            (float)zCount * size);

//        result += transform.position;

//        return result;
//    }
//    private void OnDrawGizmos()
//    {
//        if (points.Count > 0)
//        {
//            points.Clear();
//        }

//        for (int x = 0; x < m_GridSize; x++)
//        {
//            for (int z = 0; z < m_GridSize; z++)
//            {
//               points.Add(new Vector3(x - m_GridSize * 0.5f, 0, z - m_GridSize * 0.5f) + transform.position);
//            }
//        }

//        for (int i = 0; i < points.Count; i++)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawSphere(points[i], 0.1f);
//            Gizmos.color = Color.cyan;

//            if (points[i].x != m_GridSize - 1)
//            {
//                Gizmos.DrawLine(new Vector3(points[i].x, 0, points[i].z), new Vector3((points[i].x + 1), 0, points[i].z));
//            }
//            if (points[i].z != m_GridSize - 1)
//            {
//                Gizmos.DrawLine(new Vector3(points[i].x, 0, points[i].z), new Vector3(points[i].x, 0, points[i].z + 1));
//            }
//        }

//    }
//}
