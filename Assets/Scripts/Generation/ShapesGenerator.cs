using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Shapes;

public static class ShapesGenerator
{
    private const float _shapeYpos = 10.22f;

    private static int shapesCount;

    public static void StartGeneration()
    {
        shapesCount = 0;

        var astarNode = AstarPath.active.astarData.gridGraph.GetNearest(ConnectorsManager.StartConnector.transform.position).node;
        VectorInt2 nodeIndex = astarNode.position.ToVector3();
        //Debug.LogWarning(x + "," + y);
        var node = NodesGrid.Grid[nodeIndex.x, nodeIndex.y];

        GenerateRecursively(node, ConnectorsManager.StartConnector.CurrentDirection);

        int i = 0;
        bool isAllConnected = false;
        while (i < 100 && isAllConnected == false)
        {
            RandomRotateAllShapes();
            isAllConnected = ConnectorsManager.GetConnectedCount() == ConnectorsManager.TargetConnectors.Count();
            i++;
        }
        if (isAllConnected)
            Debug.LogWarning("<color=green>isAllConnected=true</color> iterations_count=" + i);

        FillEmptyNodes();
    }


    private static void GenerateRecursively(NodesGrid.Node node, Direction prevOutDirection)
    {
        if (!node.IsAvailable)
            return;

        node.SetShape(CreateTeeShape(node.X, node.Y));
        shapesCount++;
        FastRotateToConnection(node.Shape, prevOutDirection);

        //генерация ноды, если свободная клетка
        List<KeyValuePair<NodesGrid.Node,Direction>> nodes = NodesGrid.FindAvailableNeighborNodesForShapeSides(node);
        foreach (var nodeDirPair in nodes)
        {
            GenerateRecursively(nodeDirPair.Key, nodeDirPair.Value.GetOpposite());
        }
    }

    private static void RandomRotateAllShapes()
    {
        //количество подключенных коннекторов в начале этой функции
        int connectedConnectorsMaxCount = ConnectorsManager.GetConnectedCount();

        foreach (var node in NodesGrid.Grid)
        {
            if (node.Shape != null)
            {
                Direction currentDir = node.Shape.CurrentDirection;
                RandomRotateShape(node.Shape);

                int connectedCount = ConnectorsManager.GetConnectedCount();
                connectedConnectorsMaxCount = Mathf.Max(connectedConnectorsMaxCount, connectedCount);

                //возврат к состоянию до рендомного вращения
                if (connectedCount < connectedConnectorsMaxCount)
                    node.Shape.FastRotateToDirection(currentDir);
            }

            //генерация ноды, если свободная клетка
            List<KeyValuePair<NodesGrid.Node, Direction>> nodes = NodesGrid.FindAvailableNeighborNodesForShapeSides(node);
            foreach (var nodeDirPair in nodes)
            {
                GenerateRecursively(nodeDirPair.Key, nodeDirPair.Value.GetOpposite());
            }

        }
    }



    private static Shape CreateTeeShape(float x, float y)
    {
        Vector3 pos = new Vector3(x, _shapeYpos, y);
        var teeGO = (GameObject) Object.Instantiate(ResourcesLoader.TeeShapePrefab, pos, new Quaternion(0, 0, 0, 0));
        teeGO.transform.parent = SceneContainers.Shapes;
        return teeGO.GetComponent<TeeShape>();
    }

    private static Shape CreateRandomShape(float x, float y)
    {
        int rnd = Random.Range(0, 3);
        Vector3 pos = new Vector3(x, _shapeYpos, y);
        Shape retShape = null;
        GameObject prefab;
        switch (rnd)
        {
            case 0:
                prefab = ResourcesLoader.LineShapePrefab;
                break;
            case 1:
                prefab = ResourcesLoader.CornerShapePrefab;
                break;
            default:
                prefab = ResourcesLoader.TeeShapePrefab;
                break;
        }

        var shapeGO = (GameObject) Object.Instantiate(prefab, pos, new Quaternion(0, 0, 0, 0));
        retShape = shapeGO.GetComponent<Shape>();
        retShape.transform.parent = SceneContainers.Shapes;
        return retShape;
    }


    private static void RandomRotateShape(Shape shape)
    {
        Direction dir = (Direction) Random.Range(0, 4);
        shape.FastRotateToDirection(dir);
    }

    /// <summary>
    /// Вращает targetShape, пока он не будет соединен с другим, если это возможно. Если targetShape расположен в ноде с коннектором, то будет требоваться соединение еще и с коннектором
    /// </summary>
    private static void FastRotateToConnection(Shape targetShape, Direction direction)
    {
        for (int i = 0; i < 4; i++)
        {
            if (targetShape.HasConnection(direction))
            {
                var connector = GetConnector(targetShape);
                if (connector == null)
                    return;
                if (targetShape.HasConnection(connector.CurrentDirection))
                    return;
            }
           
            targetShape.FastRotate();
        }
        Debug.LogError("connection not found");
    }

    private static Connector GetConnector(Shape targetShape)
    {
        var connector = ConnectorsManager.TargetConnectors;
        return connector.FirstOrDefault(c => c.NearestShape == targetShape);
    }

    private static void FillEmptyNodes()
    {
        int needShapesCount = NodesGrid.GetNodesCountWithNotShapeObject();
        if (shapesCount == needShapesCount)
            return;

        Debug.LogWarning("<color=green>FillEmptyNodes()</color> count=" + shapesCount);

        foreach (var node in NodesGrid.Grid)
        {
            if (node.IsAvailable)
            {
                node.SetShape(CreateRandomShape(node.X, node.Y));
                shapesCount++;
            }
        }
    }

}
