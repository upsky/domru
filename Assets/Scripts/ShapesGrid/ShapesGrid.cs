using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;


public class ShapesGrid : RequiredMonoSingleton<ShapesGrid>
{
    public static Shape[,] Grid
    {
        get { return Instance._shapesGrid; }
    }

    //массив направлен снизу вверх.
    private Shape[,] _shapesGrid;


    private void Start()
    {
        _shapesGrid = FillNodesMatrix();
    }

    public static Shape GetNextShape(Shape shape, Direction dir)
    {
        if (dir == Direction.Up && shape.Yindex + 1 <= Grid.GetUpperBound(0))
            return Instance._shapesGrid[shape.Yindex + 1, shape.Xindex];

        if (dir == Direction.Right && shape.Xindex + 1 <= Grid.GetUpperBound(1))
            return Instance._shapesGrid[shape.Yindex, shape.Xindex + 1];

        if (dir == Direction.Down && shape.Yindex - 1 >= 0)
            return Instance._shapesGrid[shape.Yindex - 1, shape.Xindex];

        if (dir == Direction.Left && shape.Xindex - 1 >= 0)
            return Instance._shapesGrid[shape.Yindex, shape.Xindex - 1];

        return null;
    }

    public static List<Shape> FindConnectedNeighborShapes(Shape shape)
    {
        List<Shape> neighbors = new List<Shape>();

        if (shape.Up && shape.Yindex + 1 <= Grid.GetUpperBound(0))
        {
            var neighborShape = Grid[shape.Yindex + 1, shape.Xindex];
            if (neighborShape != null && neighborShape.Down && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Right && shape.Xindex + 1 <= Grid.GetUpperBound(1))
        {
            var neighborShape = Grid[shape.Yindex, shape.Xindex + 1];
            if (neighborShape != null && neighborShape.Left && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Down && shape.Yindex - 1 >= 0)
        {
            var neighborShape = Grid[shape.Yindex - 1, shape.Xindex];
            if (neighborShape != null && neighborShape.Up && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        if (shape.Left && shape.Xindex - 1 >= 0)
        {
            var neighborShape = Grid[shape.Yindex, shape.Xindex - 1];
            if (neighborShape != null && neighborShape.Right && !neighborShape.IsInRotateProcess)
                neighbors.Add(neighborShape);
        }

        return neighbors;
    }

    private static Shape[,] FillNodesMatrix()
    {
        var gridGraph = AstarPath.active.astarData.gridGraph;
        var shapesGrid = new Shape[gridGraph.depth,gridGraph.width];

        //Debug.LogWarning("y="+gridGraph.depth + ", x=" + gridGraph.width);

        foreach (Transform tr in SceneContainers.Shapes)
        {
            if (!tr.gameObject.activeSelf)
                continue;
            //Debug.LogWarning( Mathf.RoundToInt(tr.position.x)+","+ Mathf.RoundToInt(tr.position.z));
            var shape = tr.GetComponent<Shape>();
            int x = Mathf.RoundToInt(tr.position.x);
            int y = Mathf.RoundToInt(tr.position.z);
            //Debug.LogWarning(y + "," + x, shape);
            shapesGrid[y, x] = shape;
            shape.Xindex = x;
            shape.Yindex = y;
        }

        //Debug.LogWarning("UpperBound="+shapesGrid.GetUpperBound(0));=6
        return shapesGrid;
    }


    #region Tests methods

    private void TestOuts()
    {
        //int x = 2, y = 2; //Line
        //int x = 2, y = 3; //Corner
        int x = 6, y = 0; //Tee
        var outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Up);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Up: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Right);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Right: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Down);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Down: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Left);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Left: outDir=" + outDir, ShapesGrid.Grid[y, x]);
    }

    private void TestPath()
    {
        //int x = 2, y = 2; //Line
        int x = 2, y = 3; //Corner
        //int x = 6, y = 0; //Tee
        var path = ShapesGrid.Grid[y, x].TestPath(Direction.Up);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Up: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Right);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Right: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Down);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Down: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Left);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Left: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");
    }

    #endregion
}
