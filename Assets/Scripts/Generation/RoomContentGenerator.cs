using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Shapes;
using Random = UnityEngine.Random;

public class RoomContentGenerator : RequiredMonoSingleton<RoomContentGenerator>
{
    private class SpawnNode
    {
        public bool IsConnectorNode;
        public bool IsDeviceNode;
        public bool IsCatTargetNode;
        public NodesGrid.Node GridNode;

        /// <summary>
        /// Направление, противоположное стене. Для угловых нод, всегда за данное направление отвечает NodesGrid.Node.Y
        /// </summary>
        public Direction Direction1 = Direction.None;

        /// <summary>
        /// Дополнительное направление, противоположное второй стене.(только для угловых нод) 
        /// </summary>
        public Direction Direction2 = Direction.None;

        public SpawnNode(NodesGrid.Node gridNode)
        {
            GridNode = gridNode;
            if (gridNode.Y == 0)
                Direction1 = Direction.Up;
            else if (gridNode.Y == 6)
                Direction1 = Direction.Down;
            else if (gridNode.X == 0)
                Direction1 = Direction.Right;
            else
                Direction1 = Direction.Left;

            if (gridNode.Y == 0 || gridNode.Y == 6)
            {
                if (gridNode.X == 0)
                    Direction2 = Direction.Right;
                else if (gridNode.X == 6)
                    Direction2 = Direction.Left;
            }
        }
    }

    [SerializeField]
    private Transform _sofaPrefab;

    [SerializeField]
    private Transform _plasmaTVPrefab;

    [SerializeField]
    private Transform _connectorPrefab;

    [SerializeField]
    private Transform[] _windowPrefabs;

    private Transform _furniture;
    private Transform _devices;
    private Transform _windows;
    private List<SpawnNode> _allNodes = new List<SpawnNode>();
    private List<SpawnNode> _emptyNodes = new List<SpawnNode>();


    protected override void Awake()
    {
        base.Awake();
        _furniture = SceneContainers.RoomContent.Find("Furniture");
        _devices = SceneContainers.RoomContent.Find("Devices");
        _windows = SceneContainers.RoomContent.Find("Windows");

        if (Instance._sofaPrefab == null)
            Debug.LogException(new Exception("_sofaPrefab is null"), Instance);
        if (Instance._plasmaTVPrefab == null)
            Debug.LogException(new Exception("_plasmaTVPrefab is null"), Instance);
    }


    public static void Generate()
    {
        Instance.FillNodes();

        Direction sofaDir;
        int xIndex, yIndex;
        Vector3 sofa_pos;
        Instance.CreateSofa(out sofa_pos, out sofaDir, out xIndex, out yIndex);
        Instance.CreateWindows(sofaDir);

        Vector3 plasmaPos;
        Instance.CreatePlasma(sofa_pos, sofaDir, out plasmaPos);
        Instance.CreatePlasmaConnector(plasmaPos, sofaDir);

        Instance.CreateStartConnector(sofaDir);


        //todo префабы устройств тоже сделать списком с исключением элементов из рандома, при их добавленнии на сцену.
        //префабы устройств составные - сразу со столами.

        AstarPath.active.Scan();//AstarPath.active.astarData.gridGraph.
        NodesGrid.UpdateNodesData();
    }

    private void CreateSofa(out Vector3 position, out Direction sofaDir, out int xIndex, out int yIndex)
    {
        sofaDir = (Direction)Random.Range(0, 4); //sofaDir = Direction.Down;
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
        position = node.AstarNode.position.ToVector3();

        var sofa = (Transform)Instantiate(Instance._sofaPrefab, position, sofaQuaternion);
        GridAligner.AlignTwoNodeObject(sofa);
        sofa.parent = Instance._furniture;
    }

    private void CreatePlasma(Vector3 sofaPos, Direction sofaDir, out Vector3 plasmaPos)
    {
        Vector3 position = sofaPos;
        position.y = Instance._plasmaTVPrefab.transform.position.y + 10f;
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

        var plasmaTV = (Transform)Instantiate(Instance._plasmaTVPrefab, plasmaTV_pos, DirectionUtils.DirectionToQuaternion(dir));
        plasmaTV.parent = Instance._devices;
    }

    private void CreatePlasmaConnector(Vector3 plasmaPos, Direction sofaDir)
    {
        Vector3 position = plasmaPos;
        position.y = Instance._plasmaTVPrefab.transform.position.y + 10f;
        Direction dir = sofaDir.GetOpposite();

        switch (dir)
        {
            case Direction.Up:
            case Direction.Down:
                position.x -= 0.5f;
                break;

             case Direction.Right:
             case Direction.Left:
                position.z -= 0.5f;
                break;
        }

        var plasmaConnector_pos = new Vector3(position.x, Instance._connectorPrefab.transform.position.y + 10f, position.z);
        var plasmaConnector = (Transform)Instantiate(Instance._connectorPrefab, plasmaConnector_pos, DirectionUtils.DirectionToQuaternion(dir));
        plasmaConnector.parent = SceneContainers.Connectors;

        var nearNode = plasmaConnector.GetComponent<Connector>().NearestNode;
        var plasmaConnectorSpawnNode = Instance._allNodes.Find(c => c.GridNode == NodesGrid.Grid[nearNode.X, nearNode.Y]);
        plasmaConnectorSpawnNode.IsConnectorNode = true;
        Instance._emptyNodes.Remove(plasmaConnectorSpawnNode);
    }

    private void CreateStartConnector(Direction sofaDirection)
    {
        int spawnIndex = (sofaDirection==Direction.Down)? 0: Random.Range(0, 2);
        var spawnNode = Instance._allNodes[spawnIndex];
        var startConnector_pos = spawnNode.GridNode.AstarNode.BottomCenterPosition();
        startConnector_pos.y = Instance._connectorPrefab.transform.position.y + 10f;

        var startConnector = (Transform)Instantiate(Instance._connectorPrefab, startConnector_pos, Quaternion.LookRotation(Vector3.forward));
        startConnector.parent = SceneContainers.Connectors;
        startConnector.GetComponent<Connector>().IsStartConnector = true;

        spawnNode.IsConnectorNode = true;
        Instance._emptyNodes.Remove(spawnNode);
    }

    private void CreateWindows(Direction sofaDir)
    {
        const float localOffset = 3f;
        var prefabIndex = Random.Range(0, _windowPrefabs.Length);
        var prefab = _windowPrefabs[prefabIndex];
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
                    pos.x = localOffset - 0.65f;
                    pos.z = -100;
                    break;

                case Direction.Down:
                    pos.x = localOffset - 0.5f + Random.Range(0, 2);
                    pos.z = 100;
                    break;

                case Direction.Right:
                case Direction.Left:
                    pos.x = -100 * dir.CreateSign();
                    pos.z = localOffset - 0.5f + Random.Range(0, 2);
                    break;
            }

            var window = (Transform)Instantiate(prefab, RoomClamp(pos), DirectionUtils.DirectionToQuaternion(dir));
            window.parent = _windows;
        }
    }

    /// <summary>
    /// Добавление нод в список против часовой от ноды справа от двери [6,0]
    /// </summary>
    private void FillNodes()
    {
        _allNodes.Add(new SpawnNode(NodesGrid.Grid[6, 0]));
        for (int i = 4; i >= 0; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 0]));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[0, i]));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 6]));

        for (int i = 6; i >=1; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[6, i]));

        _emptyNodes.AddRange(_allNodes);
    }

    private static Vector3 RoomClamp(Vector3 point)
    {
        const float localOffset = 3f;
        const float min = -3.5f + localOffset;
        const float max = 3.5f + localOffset;
        point.x = Mathf.Clamp(point.x, min, max);
        point.z = Mathf.Clamp(point.z, min, max);
        return point;
    }


}
