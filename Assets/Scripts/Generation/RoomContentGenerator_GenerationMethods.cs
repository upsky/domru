using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using Shapes;
using Random = UnityEngine.Random;

public partial class RoomContentGenerator
{

    private void CreateSofa(out Vector3 position, out Direction sofaDir, out int xIndex, out int yIndex)
    {
        sofaDir = (Direction)Random.Range(0, 4); sofaDir = Direction.Left;
        Quaternion sofaQuaternion = DirectionUtils.DirectionToQuaternion(sofaDir);
        xIndex = 0;
        yIndex = 0;

        switch (sofaDir)
        {
            case Direction.Up:
                xIndex = Random.Range(2, 4);
                yIndex = Random.Range(3, 5);
                break;

            case Direction.Down:
                xIndex = 2;
                yIndex = Random.Range(2, 4);
                break;

            case Direction.Right:
                xIndex = Random.Range(3, 5);
                yIndex = Random.Range(2, 4);
                break;

            case Direction.Left:
                xIndex = Random.Range(2, 4);
                yIndex = Random.Range(2, 4);
                break;
        }

        NodesGrid.Node node = NodesGrid.Grid[xIndex, yIndex];
        position = node.Position;

        var sofa = (Transform)Instantiate(_sofaPrefab, position, sofaQuaternion);
        GridAligner.AlignTwoNodeObject(sofa);
        sofa.parent = _furniture;
    }

    private void CreatePlasma(Vector3 sofaPos, Direction sofaDir, out Vector3 plasmaPos)
    {
        Vector3 position = sofaPos;
        position.y = _plasmaTVPrefab.transform.position.y + 10f;
        Direction dir = sofaDir.GetOpposite();

        switch (dir)
        {
            case Direction.Up:
            case Direction.Down:
                position.z -= 100f * dir.CreateSign();
                position.x += 0.5f;
                break;

            case Direction.Right:
            case Direction.Left:
                position.z += 0.5f;
                position.x -= 100f * dir.CreateSign();
                break;
        }
        var plasmaTV_pos = plasmaPos = RoomClamp(position);

        var plasmaTV = (Transform)Instantiate(_plasmaTVPrefab, plasmaTV_pos, DirectionUtils.DirectionToQuaternion(dir));
        plasmaTV.parent = _devices;
    }

    private void CreatePlasmaConnector(Vector3 plasmaPos, Direction sofaDir)
    {
        Vector3 position = plasmaPos;
        position.y = _connectorPrefab.position.y + 10f;
        Direction dir = sofaDir.GetOpposite();
        int secondNodeOffsetIndexX = 0;
        int secondNodeOffsetIndexY = 0;

        switch (dir)
        {
            case Direction.Up:
            case Direction.Down:
                position.x -= 0.5f;
                secondNodeOffsetIndexX = 1;
                break;

            case Direction.Right:
            case Direction.Left:
                position.z -= 0.5f;
                secondNodeOffsetIndexY = 1;
                break;
        }

        var plasmaConnector = (Transform)Instantiate(_connectorPrefab, position, DirectionUtils.DirectionToQuaternion(dir));
        plasmaConnector.parent = SceneContainers.Connectors;

        var nearNode = plasmaConnector.GetComponent<Connector>().NearestNode;
        var plasmaConnectorSpawnNode = _emptyNodes.Find(c => c.GridNode == NodesGrid.Grid[nearNode.X, nearNode.Y]);
        RemoveNodeFromEmptyNodes(plasmaConnectorSpawnNode, SpawnNodeType.Connector);

        var secondNode = _emptyNodes.Find(c => c.GridNode == NodesGrid.Grid[nearNode.X + secondNodeOffsetIndexX, nearNode.Y + secondNodeOffsetIndexY]);
        RemoveNodeFromEmptyNodes(secondNode, SpawnNodeType.Device);
    }

    private void CreateStartConnector(Direction sofaDirection)
    {
        int spawnIndex;
        if (sofaDirection == Direction.Down)
            spawnIndex = 0;
        else if (sofaDirection == Direction.Right)
            spawnIndex = 1;
        else
            spawnIndex = Random.Range(0, 2);

        var spawnNode = _emptyNodes[spawnIndex];
        var startConnector_pos = spawnNode.GridNode.AstarNode.BottomCenterPosition();
        startConnector_pos.y = _connectorPrefab.position.y + 10f;

        var startConnector = (Transform)Instantiate(_connectorPrefab, startConnector_pos, Quaternion.LookRotation(Vector3.forward));
        startConnector.parent = SceneContainers.Connectors;
        startConnector.GetComponent<Connector>().IsStartConnector = true;

        RemoveNodeFromEmptyNodes(spawnNode, SpawnNodeType.Connector);
    }

    private void CreateConnector(SpawnNode spawnNode, Direction direction)
    {
        var startConnector_pos = spawnNode.GridNode.AstarNode.position.ToVector3();
        startConnector_pos.y = _connectorPrefab.position.y + 10f;

        switch (direction)
        {
            case Direction.Up:
            case Direction.Down:
                startConnector_pos.z -= 0.5f*direction.CreateSign();
                break;

            case Direction.Left:
            case Direction.Right:
                startConnector_pos.x -= 0.5f * direction.CreateSign();
                break;
        }

        var connector = (Transform)Instantiate(_connectorPrefab, startConnector_pos, DirectionUtils.DirectionToQuaternion(direction));
        connector.parent = SceneContainers.Connectors;

        RemoveNodeFromEmptyNodes(spawnNode, SpawnNodeType.Connector);
    }

    private void CreateWindows(Direction sofaDir)
    {
        const float localOffset = 3f;
        var prefab = RandomUtils.GetRandomItem(_windowPrefabs);
        int winCount = Random.Range(1, 3);

        int prevDir = -1;
        for (int i = 0; i < winCount; i++)
        {
            Direction dir = (Direction)RandomUtils.RangeWithExclude(0, 4, (int)sofaDir.GetOpposite(), prevDir);
            prevDir = (int) dir;

            Vector3 pos = new Vector3(0f, prefab.position.y + 10f, 0f);
            switch (dir)
            {
                case Direction.Up:
                    pos.x = localOffset - 0.5f;//0.65f;
                    pos.z = -100f;
                    break;

                case Direction.Down:
                    pos.x = localOffset - 0.5f + Random.Range(0, 2);
                    pos.z = 100f;
                    break;

                case Direction.Right:
                case Direction.Left:
                    pos.x = -100 * dir.CreateSign();
                    pos.z = localOffset - 0.5f + Random.Range(0, 2);
                    break;
            }

            pos = RoomClamp(pos);
            var window = (Transform)Instantiate(prefab, pos, DirectionUtils.DirectionToQuaternion(dir));
            window.parent = _windows;

            var nearNode = AstarPath.active.astarData.gridGraph.GetNearest(pos - new Vector3(0.1f, 0, 0.1f)).node;
            var spawnNode = _emptyNodes.Find(c => c.GridNode.AstarNode == nearNode);
            RemoveNodeFromEmptyNodes(spawnNode, SpawnNodeType.Busy);

            var nearNode2 = AstarPath.active.astarData.gridGraph.GetNearest(pos + new Vector3(0.1f, 0, 0.1f)).node;
            var spawnNode2 = _emptyNodes.Find(c => c.GridNode.AstarNode == nearNode2);
            RemoveNodeFromEmptyNodes(spawnNode2, SpawnNodeType.Busy);
        }
    }


    private void CreateComp()
    {
        var compSpawnNode = GetFarEmptyNodeFrom(SpawnNodeType.Connector);

        var compPrefab = RandomUtils.GetRandomItem(_compPrefabs);
        var comp = (Transform)Instantiate(compPrefab, compSpawnNode.GridNode.Position, DirectionUtils.DirectionToQuaternion(compSpawnNode.Direction1));
        comp.parent = _devices;

        RemoveNodeFromEmptyNodes(compSpawnNode, SpawnNodeType.Device);

        var connectorSpawnNode = GetFarthestFromConnectorstNeighborNode(compSpawnNode);
        Direction connectorDir = compSpawnNode.Direction2 == Direction.None ? compSpawnNode.Direction1 : connectorSpawnNode.Direction1;
        CreateConnector(connectorSpawnNode, connectorDir);
    }

    private void CreatePhone()
    {
        var spawnNode = GetFarEmptyNodeFrom(SpawnNodeType.Connector);

        Vector3 pos = spawnNode.GridNode.Position;
        pos.y = _phonePrefab.position.y + 10f;
        switch (spawnNode.Direction1)
        {
            case Direction.Up:
            case Direction.Down:
                pos.z -= 0.5f * spawnNode.Direction1.CreateSign();
                break;

            case Direction.Left:
            case Direction.Right:
                pos.x -= 0.5f * spawnNode.Direction1.CreateSign();
                break;
        }

        var phone = (Transform)Instantiate(_phonePrefab, pos, DirectionUtils.DirectionToQuaternion(spawnNode.Direction1));
        phone.parent = _devices;

        //SpawnNodeType.Device  - не нужно, т.к. не мешает коту
        CreateConnector(spawnNode, spawnNode.Direction1);
    }

    //todo если нужно конкретные элементы расположить, то это недолго, а пока рендомно префабы генерить
    private void CreateCovers()
    {
        int count = Random.Range(3, 6);
        var cornerNodes = GetEmptyCornerNodes(_emptyNodes);
        foreach (var spawnNode in cornerNodes)
        {
            var prefab = RandomUtils.GetRandomItem(_coversPrefabs);
            Direction dir = spawnNode.Direction1;

            Vector3 pos = spawnNode.GridNode.Position;
            pos.y += 0.22f;
            var cover = (Transform)Instantiate(prefab, pos, DirectionUtils.DirectionToQuaternion(dir));
            cover.parent = _furniture;

            RemoveNodeFromEmptyNodes(spawnNode, SpawnNodeType.Cover);
        }

        count -= cornerNodes.Count;

        for (int i = 0; i < count; i++)
        {
            //var spawnNode = RandomUtils.GetRandomItem(_emptyNodes);//GetFarEmptyNode();//_emptyNodes.
            //todo для кота и cover юзать аналог GetFarEmptyNodeFromConnectors()
            //Для cover юзать GetFarEmptyNodeFrom(excmudeNodetype = cover)
            //Для cover юзать GetFarEmptyNodeFrom(excmudeNodetype = device)
            var spawnNode = GetFarEmptyNodeFrom(SpawnNodeType.Cover);


            var prefab = RandomUtils.GetRandomItem(_coversPrefabs);
            Direction dir = spawnNode.Direction1;

            Vector3 pos = spawnNode.GridNode.Position;
            pos.y += 0.22f;
            var cover = (Transform)Instantiate(prefab, pos, DirectionUtils.DirectionToQuaternion(dir));
            cover.parent = _furniture;

            RemoveNodeFromEmptyNodes(spawnNode, SpawnNodeType.Cover);
        }
    }


}
