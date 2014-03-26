using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;


public class NodesGrid : RequiredMonoSingleton<NodesGrid>
{
    public class Node
    {
        //public int x;
        //public int y;
        public Shape shape;
        public GameObject notShapeObject;//не Shape
        //public Device device; //под вопросом. 

        /// <summary>
        /// Проверяет, не занята ли клетка каким-нибудь объектом
        /// </summary>
        public bool IsAvailable
        {
            get { return shape == null && notShapeObject == null;/* && device==null*/ }
        } 
    }


    public static Node[,] Grid
    {
        get { return Instance._nodesGrid; }
    }

    //массив направлен снизу вверх.
    private Node[,] _nodesGrid;


    private void Start()
    {
        _nodesGrid = FillNodesMatrix();
    }

    public static Shape GetNextShape(Shape shape, Direction dir)
    {
        if (dir == Direction.Up && shape.Yindex + 1 <= Grid.GetUpperBound(0))
            return Instance._nodesGrid[shape.Yindex + 1, shape.Xindex].shape;

        if (dir == Direction.Right && shape.Xindex + 1 <= Grid.GetUpperBound(1))
            return Instance._nodesGrid[shape.Yindex, shape.Xindex + 1].shape;

        if (dir == Direction.Down && shape.Yindex - 1 >= 0)
            return Instance._nodesGrid[shape.Yindex - 1, shape.Xindex].shape;

        if (dir == Direction.Left && shape.Xindex - 1 >= 0)
            return Instance._nodesGrid[shape.Yindex, shape.Xindex - 1].shape;

        return null;
    }

    public static List<Shape> FindConnectedNeighborShapes(Shape shape)
    {
        List<Shape> neighbors = new List<Shape>();

        if (shape.Up && shape.Yindex + 1 <= Grid.GetUpperBound(0))
        {
            var neighborShape = Grid[shape.Yindex + 1, shape.Xindex].shape;
            if (neighborShape != null && neighborShape.Down && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Right && shape.Xindex + 1 <= Grid.GetUpperBound(1))
        {
            var neighborShape = Grid[shape.Yindex, shape.Xindex + 1].shape;
            if (neighborShape != null && neighborShape.Left && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Down && shape.Yindex - 1 >= 0)
        {
            var neighborShape = Grid[shape.Yindex - 1, shape.Xindex].shape;
            if (neighborShape != null && neighborShape.Up && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Left && shape.Xindex - 1 >= 0)
        {
            var neighborShape = Grid[shape.Yindex, shape.Xindex - 1].shape;
            if (neighborShape != null && neighborShape.Right && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        return neighbors;
    }

    private static Node[,] FillNodesMatrix()
    {
        var gridGraph = AstarPath.active.astarData.gridGraph;
        var nodesGrid = new Node[gridGraph.depth,gridGraph.width];

        for (int i = 0; i < gridGraph.depth; i++)
            for (int j = 0; j < gridGraph.width; j++)
            {
                var node = new Node();

                int ypos = Mathf.RoundToInt(AstarPath.active.astarData.gridGraph.center.y);
                var collaiders = Physics.OverlapSphere(new Vector3(j, ypos, i), 0.499f);
                collaiders = collaiders.Where(
                    c => //c.CompareTag(Consts.Tags.nodeDevice) ||
                    c.CompareTag(Consts.Tags.nodeFurniture) ||
                    c.CompareTag(Consts.Tags.shape)).ToArray();

                if (collaiders.Count() > 1)
                {
                    Debug.LogError(i + "," + j + " count=" + collaiders.Count());
                    continue;
                }

                if (collaiders.Length > 0)
                {
                    var c = collaiders.First();
                    Shape shape = c.GetComponent<Shape>();
                    if (shape != null)
                    {                        
                        int x = Mathf.RoundToInt(shape.transform.position.x);
                        int y = Mathf.RoundToInt(shape.transform.position.z);
                        shape.Xindex = x; //j;
                        shape.Yindex = y; //i;
                        node.shape = shape;
                    }
                    else
                        node.notShapeObject = c.gameObject;
                }
                nodesGrid[i, j] = node;
            }

        //Debug.LogWarning("y="+gridGraph.depth + ", x=" + gridGraph.width);

        //foreach (Transform tr in SceneContainers.Shapes)
        //{
        //    if (!tr.gameObject.activeSelf)
        //        continue;
        //    //Debug.LogWarning( Mathf.RoundToInt(tr.position.x)+","+ Mathf.RoundToInt(tr.position.z));
        //    var shape = tr.GetComponent<Shape>();
        //    int x = Mathf.RoundToInt(tr.position.x);
        //    int y = Mathf.RoundToInt(tr.position.z);
        //    //Debug.LogWarning(y + "," + x, shape);
        //    nodesGrid[y, x].shape = shape;
        //    shape.Xindex = x;
        //    shape.Yindex = y;
        //}

        //Debug.LogWarning("UpperBound="+shapesGrid.GetUpperBound(0));=6
        return nodesGrid;
    }

    /*
    #region Tests methods

    private void TestOuts()
    {
        //int x = 2, y = 2; //Line
        //int x = 2, y = 3; //Corner
        int x = 6, y = 0; //Tee
        var outDir = NodesGrid.Grid[y, x].GetOutDirection(Direction.Up);
        Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Up: outDir=" + outDir, NodesGrid.Grid[y, x]);

        outDir = NodesGrid.Grid[y, x].GetOutDirection(Direction.Right);
        Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Right: outDir=" + outDir, NodesGrid.Grid[y, x]);

        outDir = NodesGrid.Grid[y, x].GetOutDirection(Direction.Down);
        Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Down: outDir=" + outDir, NodesGrid.Grid[y, x]);

        outDir = NodesGrid.Grid[y, x].GetOutDirection(Direction.Left);
        Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Left: outDir=" + outDir, NodesGrid.Grid[y, x]);
    }

    private void TestPath()
    {
        //int x = 2, y = 2; //Line
        int x = 2, y = 3; //Corner
        //int x = 6, y = 0; //Tee
        var path = NodesGrid.Grid[y, x].TestPath(Direction.Up);
        foreach (var pointName in path)
            Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Up: " + pointName, NodesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = NodesGrid.Grid[y, x].TestPath(Direction.Right);
        foreach (var pointName in path)
            Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Right: " + pointName, NodesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = NodesGrid.Grid[y, x].TestPath(Direction.Down);
        foreach (var pointName in path)
            Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Down: " + pointName, NodesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = NodesGrid.Grid[y, x].TestPath(Direction.Left);
        foreach (var pointName in path)
            Debug.LogWarning(NodesGrid.Grid[y, x].GetType() + " InDir=Left: " + pointName, NodesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");
    }

    #endregion
    */
}
