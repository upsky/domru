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

        if (_needRoomContentGeneration)
        {
            RoomContentGenerator.Generate();
            EventMessenger.SendMessage(GameEvent.CompleteContentGeneration, this);
        }

        if (_needShapesGeneration)
        {
            ShapesGenerator.Generate();
            EventMessenger.SendMessage(GameEvent.CompleteNodesGeneration, this);
        }
        StartGameProcess();   
	}

    //private void OnDrawGizmos()
    //{
    //    for (float i = 30; i >= 0; i-=0.2f)
    //    {
    //        int x = (int) Mathf.Pow(1.2589254117941672104239541063958f, i);
    //        Debug.LogWarning(1000-x);
    //    }
    //}


    private void StartGameProcess()
    {
        //Debug.LogWarning("StartGame");
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
        bool isUsedHelp = (CurrentGameMode == GameMode.InvokeAdjuster);

        //Debug.LogWarning("<color=green>VICTORY</color>");
        CurrentGameMode = GameMode.Victory;
        EventMessenger.SendMessage(GameEvent.EngGameProcess, this);

        var victoryPanel = _UIRoot.transform.Find("VictoryPanel");
        NGUITools.SetActive(victoryPanel.gameObject, true);
        var timer = _UIRoot.GetComponentsInChildren<LabelTimer>().First();
        timer.enabled = false;

        long score = isUsedHelp ? 100 : ScoreCounter.TimeToStore(timer.StartTime, timer.RemainTime);
        MySocial.SubmitScore(score);


        var scoreLabel = _UIRoot.transform.FindChild("MainGamePanel/01_spriteForText/02_lblScore");
        scoreLabel.GetComponent<UILabel>().text = score.ToString();
        var scoreLabelInVictoryPanel = _UIRoot.transform.FindChild("VictoryPanel/02_spriteTop/03_lblScore");
        scoreLabelInVictoryPanel.GetComponent<UILabel>().text = score.ToString();
    }

    private bool CheckVictoryCondition()
    {
        return ConnectorsManager.TargetConnectors.Length == ConnectorsManager.TargetConnectors.Count(c => c.IsConnected);
    }

}
