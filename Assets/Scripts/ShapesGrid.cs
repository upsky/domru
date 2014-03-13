using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;


public class ShapesGrid : MonoSingleton<ShapesGrid>
{

    public static Shape[,] Grid
    {
        get { return Instance._shapesGrid; }
    }

    //массив направлен снизу вверх.
    private Shape[,] _shapesGrid;

    [HideInInspector]
    public Connector StartConnector = null;

    private List<Connector> targetConnectors = new List<Connector>();

    //private List<Connector> _connectedConnectors = new List<Connector>();
    private List<Connector> _unConnectedConnectors = new List<Connector>();

    private List<Shape> _traversedShapes = new List<Shape>();

    private void Start()
    {
        _shapesGrid = FillNodesMatrix();

        //Init connectors
        foreach (Transform connector in SceneContainers.Connectors)
        {
            var c = connector.GetComponent<Connector>();
            if (c.IsStartConnector)
            {
                if (StartConnector==null)
                    StartConnector = c;
                else
                    Debug.LogError("Флаг `IsStartConnector=true` больше чем у одного коннектора в сцене");
            }
            else
            {
                targetConnectors.Add(c);
            }
        }

        if (StartConnector == null)
            Debug.LogError("StartConnector not found");
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

    //Потом перенести этот метод в GameSceneManager. А также ссылки на коннекторы (targetConnectors, StartConnector)
    public void CheckAllConnections()
    {
        _unConnectedConnectors.AddRange(targetConnectors);

        //if Has Start Connection
        if (StartConnector.NearestShape.HasConnection(StartConnector.CurrentDirection))
        {
            CheckConnectRecursively(StartConnector.NearestShape);
            _traversedShapes.Clear();
        }

        foreach (var c in _unConnectedConnectors)
            c.SwitchToOff();

        //_connectedConnectors.Clear();
        _unConnectedConnectors.Clear();
    }

    private void CheckConnectRecursively(Shape shape)
    {
        _traversedShapes.Add(shape);
      

        var connector = FindConnectorWithConnection(shape);
        if (connector != null)
        {
            if (shape.HasConnection(connector.CurrentDirection))
            {
                connector.SwitchToOn();//todo потом убрать включение отсюда, т.к. включать могут только сигналы при достижении коннектора 
                _unConnectedConnectors.Remove(connector);
                //_connectedConnectors.Add(connector);
            }
        }

        var neighbors = FindConnectedNeighborShapes(shape);
        //Debug.LogWarning(shape.Yindex + ", " + shape.Xindex + ",  neighborsCount=" + neighbors.Count());
        foreach (var neighbor in neighbors)
        {
            if (!_traversedShapes.Contains(neighbor))
                CheckConnectRecursively(neighbor);
        }
    }

    private List<Shape> FindConnectedNeighborShapes(Shape shape)
    {
        List<Shape> neighbors = new List<Shape>();

        if (shape.Up && shape.Yindex + 1 <= Grid.GetUpperBound(0))
        {
            var neighborShape = _shapesGrid[shape.Yindex + 1, shape.Xindex];
            if (neighborShape!=null && neighborShape.Down)
                neighbors.Add(neighborShape);
        }

        if (shape.Right && shape.Xindex + 1 <= Grid.GetUpperBound(1))
        {
            var neighborShape = _shapesGrid[shape.Yindex, shape.Xindex + 1];
            if (neighborShape != null && neighborShape.Left)
                neighbors.Add(neighborShape);
        }

        if (shape.Down && shape.Yindex - 1 >= 0)
        {
            var neighborShape = _shapesGrid[shape.Yindex - 1, shape.Xindex];
            if (neighborShape != null && neighborShape.Up)
                neighbors.Add(neighborShape);
        }

        if (shape.Left && shape.Xindex - 1 >= 0)
        {            
            var neighborShape = _shapesGrid[shape.Yindex, shape.Xindex - 1];
            if (neighborShape != null && neighborShape.Right)
                neighbors.Add(neighborShape);
        }

        return neighbors;
    }

    private Connector FindConnectorWithConnection(Shape shape)
    {
        return _unConnectedConnectors.FirstOrDefault(c => c.NearestShape == shape);
    }

    private static Shape[,] FillNodesMatrix()
    {
        var gridGraph = AstarPath.active.astarData.gridGraph;
        var shapesGrid = new Shape[gridGraph.width, gridGraph.depth];

        foreach (Transform tr in SceneContainers.Shapes)
        {
            if (!tr.gameObject.activeSelf)
                continue;
            //Debug.LogWarning( Mathf.RoundToInt(tr.position.x)+","+ Mathf.RoundToInt(tr.position.z));
            var shape = tr.GetComponent<Shape>();
            int x = Mathf.RoundToInt(tr.position.x);
            int y = Mathf.RoundToInt(tr.position.z);

            shapesGrid[y, x] = shape;
            shape.Xindex = x;
            shape.Yindex = y;
        }

        //Debug.LogWarning("UpperBound="+shapesGrid.GetUpperBound(0));=6
        return shapesGrid;
    }

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
}
