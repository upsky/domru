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

        EventMessenger.Subscribe(GameEvent.ConnetorSwitchToOn, this, OnConnetorSwitchToOn);        
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
        NGUITools.SetActive(Instance._victoryPanel, true);
        NGUITools.GetRoot(Instance._victoryPanel).GetComponentsInChildren<LabelTimer>().First().enabled = false;
    }

    private bool CheckVictoryCondition()
    {
        return ConnectorsManager.TargetConnectors.Length == ConnectorsManager.TargetConnectors.Count(c => c.IsConnected);
    }

}
