﻿using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;

public class MainSceneManager : RequiredMonoSingleton<MainSceneManager>
{
    public enum GameMode
    {
        //Loading,
        Normal,
        InvokeAdjuster, //режим вызова монтажника
        Victory
    }

    [SerializeField]
    private bool _needRoomContentGeneration = false;

    [SerializeField]
    private bool _needShapesGeneration = true;

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

    [SerializeField]
    private GameObject _victoryPanel;

    public static GameMode CurrentGameMode
    {
        get { return Instance._currentGameMode; }
        set { Instance._currentGameMode = value; }
    }

    private GameMode _currentGameMode;

    private void Start ()
	{
        if (_needShapesGeneration)
        {
            ShapesGenerator.Generate();
            EventMessenger.SendMessage(GameEvent.CompleteNodesGeneration, this);
        }
        StartGameProcess();
	}

    private void StartGameProcess()
    {
        Debug.LogWarning("StartGame");
        CurrentGameMode = GameMode.Normal;
        EventMessenger.SendMessage(GameEvent.StartGameProcess, this);
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

    private static void Victory()
    {        
        //Debug.LogWarning("<color=green>VICTORY</color>");
        CurrentGameMode = GameMode.Victory;
        Cat.StopAnyActivity();
        NGUITools.SetActive(Instance._victoryPanel, true);
        NGUITools.GetRoot(Instance._victoryPanel).GetComponentsInChildren<LabelTimer>().First().enabled = false;
    }

    private static bool CheckVictoryCondition()
    {
        return ConnectorsManager.TargetConnectors.Length == ConnectorsManager.TargetConnectors.Count(c => c.IsConnected);
    }

}
