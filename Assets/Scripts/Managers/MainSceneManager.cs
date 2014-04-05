using Shapes;
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

    private GameObject _UIRoot;

    public static GameMode CurrentGameMode
    {
        get { return Instance._currentGameMode; }
        set { Instance._currentGameMode = value; }
    }

    private GameMode _currentGameMode;

    private void Start ()
    {
        EventMessenger.Subscribe(GameEvent.ConnetorSwitchToOn, this, OnConnetorSwitchToOn);

        _UIRoot = GameObject.Find("UI Root");
        if (_UIRoot == null)
            Debug.LogError("UI Root not found");

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

    private void OnConnetorSwitchToOn()
    {
        if (Instance.CheckVictoryCondition())
            Instance.Victory();
    }

    public static void OnShapeRotateStart(Shape shape)
    {
        SignalsUtils.DestroySignalsInCell(shape.Xindex, shape.Yindex);
        if (CurrentGameMode != GameMode.InvokeAdjuster)
            ConnectorsManager.CheckAllConnections();
    }



    private void Victory()
    {        
        //Debug.LogWarning("<color=green>VICTORY</color>");
        CurrentGameMode = GameMode.Victory;
        EventMessenger.SendMessage(GameEvent.EngGameProcess, this);

        var victoryPanel = _UIRoot.transform.Find("VictoryPanel");
        NGUITools.SetActive(victoryPanel.gameObject, true);
        _UIRoot.GetComponentsInChildren<LabelTimer>().First().enabled = false;
    }

    private bool CheckVictoryCondition()
    {
        return ConnectorsManager.TargetConnectors.Length == ConnectorsManager.TargetConnectors.Count(c => c.IsConnected);
    }

}
