﻿using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;

public class MainSceneManager : MonoSingleton<MainSceneManager>
{
    public enum GameMode
    {
        Normal,
        InvokeAdjuster, //режим вызова монтажника
        Victory
    }

    [SerializeField]
    private Transform _signalPrefab;

    [SerializeField]
    private AdjusterController _adjuster;

    [SerializeField]
    private CatController _cat;

    [SerializeField]
    private DoorController _door;

    public static AdjusterController Adjuster
    {
        get { return Instance._adjuster; }
    }

    public static CatController Cat
    {
        get { return Instance._cat; }
    }

    public static DoorController Door
    {
        get { return Instance._door; }
    }


    public static Transform SignalPrefab
    {
        get { return Instance._signalPrefab; }
    }

    public static GameMode CurrentGameMode
    {
        get { return Instance._currentGameMode; }
        set { Instance._currentGameMode = value; }
    }

    private GameMode _currentGameMode;

    private void Start ()
	{
	    InvokeRepeating("CreateSignal", 1f, 7f);
	}

    public static void OnShapeRotateStart(Shape shape)
    {
        SignalsUtils.DestroySignalsInCell(shape.Xindex, shape.Yindex);
        if (CurrentGameMode != GameMode.InvokeAdjuster)
            ConnectorsManager.CheckAllConnections();
    }

    public static void OnConnetorSwitchToOn()
    {
        if (CheckVictoryCondition())
            Victory();
    }

    public static void Victory()
    {
        Debug.LogWarning("<color=green>VICTORY</color>");
        CurrentGameMode = GameMode.Victory;
        Cat.StopAnyActivity();
    }

    private void CreateSignal()
    {
        Vector3 pos = ConnectorsManager.StartConnector.transform.position;
        var signalGO = (Instantiate(SignalPrefab, pos, new Quaternion(0,0,0,0)) as Transform);
        var signal = signalGO.GetComponent<Signal>();
        signal.Init(ConnectorsManager.StartConnector.CurrentDirection);
    }

    private static bool CheckVictoryCondition()
    {
        return ConnectorsManager.TargetConnectors.Length == ConnectorsManager.TargetConnectors.Count(c => c.IsConnected);
    }

}
