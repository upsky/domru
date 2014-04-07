using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Shapes;
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

    private Transform _furniture;
    private Transform _devices;

    private List<SpawnNode> _allNodes = new List<SpawnNode>();
    private List<SpawnNode> _emptyNodes = new List<SpawnNode>();

    protected override void Awake()
    {
        base.Awake();
        _furniture = SceneContainers.RoomContent.Find("Furniture");
        _devices = SceneContainers.RoomContent.Find("Devices");

        //SceneContainers.Connectors - уже есть

        if (Instance._sofaPrefab == null)
            Debug.LogException(new Exception("_sofaPrefab is null"), Instance);
        if (Instance._plasmaTVPrefab == null)
            Debug.LogException(new Exception("_plasmaTVPrefab is null"), Instance);
    }


    public static void Generate()
    {
        Instance.FillNodes();

        int yIndex = UnityEngine.Random.Range(2, 4);
        NodesGrid.Node node = NodesGrid.Grid[3, yIndex];
        Vector3 sofa_pos = node.AstarNode.position.ToVector3();

        var sofa = (Transform) Instantiate(Instance._sofaPrefab, sofa_pos, new Quaternion(0, 0, 0, 0));
        GridAligner.AlignTwoNodeObject(sofa);
        sofa.parent = Instance._furniture;
        node.NotShapeObject = sofa.gameObject;
        NodesGrid.Grid[3, yIndex + 1].NotShapeObject = sofa.gameObject;


        var plasmaTV_pos = new Vector3(sofa_pos.x - 3.5f, Instance._plasmaTVPrefab.transform.position.y + 10f, sofa_pos.z + 0.5f);
        var plasmaTV = (Transform) Instantiate(Instance._plasmaTVPrefab, plasmaTV_pos, Quaternion.LookRotation(Vector3.right));
        plasmaTV.parent = Instance._devices;

        var plasmaConnector_pos = new Vector3(plasmaTV_pos.x, Instance._connectorPrefab.transform.position.y + 10f, plasmaTV_pos.z - 0.5f);
        var plasmaConnector = (Transform)Instantiate(Instance._connectorPrefab, plasmaConnector_pos, Quaternion.LookRotation(Vector3.right));
        plasmaConnector.parent = SceneContainers.Connectors;

        var plasmaConnectorSpawnNode = Instance._allNodes.Find(c => c.GridNode == NodesGrid.Grid[0, yIndex + 1]);
        plasmaConnectorSpawnNode.IsConnectorNode = true;
        Instance._emptyNodes.Remove(plasmaConnectorSpawnNode);



        //start connector
        int spawnIndex = UnityEngine.Random.Range(0, 2);
        var spawnNode = Instance._allNodes[spawnIndex];
        var startConnector_pos = spawnNode.GridNode.AstarNode.BottomCenterPosition();
        startConnector_pos.y = Instance._connectorPrefab.transform.position.y + 10f;

        var startConnector = (Transform)Instantiate(Instance._connectorPrefab, startConnector_pos, Quaternion.LookRotation(Vector3.forward));
        startConnector.parent = SceneContainers.Connectors;
        startConnector.GetComponent<Connector>().IsStartConnector = true;

        spawnNode.IsConnectorNode = true;
        Instance._emptyNodes.Remove(spawnNode);
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
}
