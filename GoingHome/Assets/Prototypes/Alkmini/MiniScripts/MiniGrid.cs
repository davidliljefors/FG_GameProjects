//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Utils;

//public class MiniGrid
//{
//    private int width;
//    private float height;
//    private int depth;
//    private int[,] gridArray;
//    public float cellSize;
//    private Transform parent;
//    private TextMesh[,] debugTextArray;

//    public int xOriginal;
//    public int zOriginal;
//    public List<Vector3> availableTiles = new List<Vector3>();
//    public Vector3 tile;
//    private Vector3 currentTile;
//    public List<Vector3> ats = new List<Vector3>();

//    //     public Dictionary<>

//    public MiniGrid(int width, float height, int depth, float cellSize, Transform parent)
//    {
//        this.width = width;
//        this.depth = depth;
//        this.cellSize = cellSize;
//        this.parent = parent;
//        Debug.Log("Created Mini Grid");
//        gridArray = new int[width, depth];
//        debugTextArray = new TextMesh[width, depth];
//        this.height = height;

//        for (int xOriginal = 0; xOriginal < gridArray.GetLength(0); xOriginal++)
//        {
//            for (int zOriginal = 0; zOriginal < gridArray.GetLength(1); zOriginal++)
//            {
//                //to be placed on string text if want to display number on grid
//                //gridArray[xOriginal, zOriginal].ToString()
//                debugTextArray[xOriginal, zOriginal] = Utils.WorldText.CreateWorldText(default, this.parent, GetWorldPosition(xOriginal, zOriginal) + new Vector3(cellSize, 0, cellSize) * 0.5f, 10, Color.magenta, TextAnchor.MiddleCenter, TextAlignment.Center);
//                currentTile = GetWorldPosition(xOriginal, zOriginal);

//                availableTiles.Add(currentTile);
//                //Debug.Log(availableTiles.Count + " : " + availableTiles[availableTiles.Count - 1]);
//                debugTextArray[xOriginal, zOriginal].transform.rotation = Quaternion.Euler(90, 0, 0);

//                Debug.DrawLine(GetWorldPosition(xOriginal, zOriginal), GetWorldPosition(xOriginal, zOriginal + 1), Color.cyan, 100f);
//                Debug.DrawLine(GetWorldPosition(xOriginal, zOriginal), GetWorldPosition(xOriginal + 1, zOriginal), Color.cyan, 100f);
//            }
//        }
//        Debug.DrawLine(GetWorldPosition(0, depth), GetWorldPosition(width, depth), Color.cyan, 100f);
//        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, depth), Color.cyan, 100f);
//        //SetValue(2, 1, 56);
//    }

//    public Vector3 GetWorldPosition(int x, int z)
//    {
//        return new Vector3(x, height, z) * cellSize + parent.position;
//    }

//    private void GetXZ(Vector3 worldPosition, out int x, out int z)
//    {
//        x = Mathf.FloorToInt((worldPosition.x - parent.position.x) / cellSize);
//        z = Mathf.FloorToInt((worldPosition.z - parent.position.z) / cellSize);
//    }

//    public void SetValue(int x, int z, int value)
//    {
//        if (x >= 0 && z >= 0 && x < width && z < depth)
//        {

//            gridArray[x, z] = value;
//            debugTextArray[x, z].text = gridArray[x, z].ToString();
//        }
//    }

//    public void SetValue(Vector3 worldPosition, int value)
//    {
//        int x, z;
//        GetXZ(worldPosition, out x, out z);
//        SetValue(x, z, value);

//    }

//    public int GetValue(int x, int z)
//    {
//        if (x >= 0 && z >= 0 && x < width && z < depth)
//        {

//            return gridArray[x, z];
//        }
//        else
//        {
//            return 0;
//        }
//    }

//    public int GetValue(Vector3 worldPosition)
//    {
//        int x, z;
//        GetXZ(worldPosition, out x, out z);
//        return GetValue(x, z);
//    }

//}
