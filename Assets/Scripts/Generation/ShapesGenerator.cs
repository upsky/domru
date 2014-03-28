using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Shapes;

public static class ShapesGenerator
{
    private const float _shapeYpos = 10.22f;

    public static void StartGeneration()
    {
        var astarNode = AstarPath.active.astarData.gridGraph.GetNearest(ConnectorsManager.StartConnector.transform.position).node;
        int x = Mathf.RoundToInt(astarNode.position.ToVector3().x);
        int y = Mathf.RoundToInt(astarNode.position.ToVector3().z);
        
        //Debug.LogWarning(x + "," + y);
        var node = NodesGrid.Grid[x, y];

        GenerateRecursively(node, ConnectorsManager.StartConnector.CurrentDirection);

        RandomRoatateAllShapes();
    }

    private static void GenerateRecursively(NodesGrid.Node node, Direction prevOutDirection)
    {
        if (!node.IsAvailable)
            return;

        node.SetShape(CreateTeeShape(node.X, node.Y));
        node.Shape.transform.parent = SceneContainers.Shapes;
        FastRotateToConnection(node.Shape, prevOutDirection);

        List<KeyValuePair<NodesGrid.Node,Direction>> nodes = NodesGrid.FindAvailableNeighborNodesForShapeSides(node);
        foreach (var nodeDirPair in nodes)
        {
            GenerateRecursively(nodeDirPair.Key, nodeDirPair.Value.GetOpposite());
        }
    }

    private static void RandomRoatateAllShapes()
    {
        foreach (var node in NodesGrid.Grid)
        {
            if (node.Shape != null)
            {
                Direction currentDir = node.Shape.CurrentDirection;
                RandomRotateShape(node.Shape);
                if (!ConnectorsManager.IsAllConnected())
                {
                    node.Shape.FastRotateToDirection(currentDir);
                }
            }
        }
    }










    private static Shape CreateTeeShape(float x, float y)
    {
        Vector3 pos = new Vector3(x, _shapeYpos, y);
        var teeGO = Object.Instantiate(ResourcesLoader.TeeShapePrefab, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
        return teeGO.GetComponent<TeeShape>();
    }

    private static Shape CreateRandomShape(float x, float y)
    {
        int rnd = Random.Range(0, 3);
        Vector3 pos = new Vector3(x, _shapeYpos, y);
        switch (rnd)
        {
            case 0:
                var lineGO = Object.Instantiate(ResourcesLoader.LineShapePrefab, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
                return lineGO.GetComponent<LineShape>();
            case 1:
                var cornerGO = Object.Instantiate(ResourcesLoader.CornerShapePrefab, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
                return cornerGO.GetComponent<CornerShape>();
            default:
                var teeGO = Object.Instantiate(ResourcesLoader.TeeShapePrefab, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
                return teeGO.GetComponent<TeeShape>();
        }
    }


    private static void RandomRotateShape(Shape shape)
    {
        Direction dir = (Direction) Random.Range(0, 4);
        shape.FastRotateToDirection(dir);
    }

    /// <summary>
    /// Вращает targetShape, пока он не будет соединен с другим, если это возможно.
    /// </summary>
    private static void FastRotateToConnection(Shape targetShape, Direction direction)
    {
        for (int i = 0; i < 4; i++)
        {
            if (targetShape.HasConnection(direction))
                return;
            targetShape.FastRotate();
        }
        Debug.LogError("connection not found");
    }

    ////добавляет соединенный shape к shape в текущем node
    //public static Shape AddConnectedShape(NodesGrid.Node node)
    //{
    //    //CreateRandomShape();
    //}

}
