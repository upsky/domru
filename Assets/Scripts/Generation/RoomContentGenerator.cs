using System;
using UnityEngine;
using System.Collections;

public class RoomContentGenerator : RequiredMonoSingleton<RoomContentGenerator>
{
    [SerializeField]
    private Transform _sofaPrefab;


    private Transform _furniture;

    protected override void Awake()
    {
        base.Awake();
        _furniture = SceneContainers.RoomContent.Find("Furniture");
    }


    public static void Generate()
    {
        if (Instance._sofaPrefab == null)
            Debug.LogException(new Exception("_sofaPrefab is null"), Instance);

        int yIndex=UnityEngine.Random.Range(2, 4);

        NodesGrid.Node node = NodesGrid.Grid[3, yIndex];
        Vector3 pos = node.AstarNode.position.ToVector3();

        var tr = (Transform)Instantiate(Instance._sofaPrefab, pos, new Quaternion(0, 0, 0, 0));
        tr.parent = Instance._furniture;

        node.NotShapeObject = tr.gameObject;
        NodesGrid.Grid[3, yIndex+1].NotShapeObject=tr.gameObject;
    }
}
