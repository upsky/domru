using Shapes;
using UnityEngine;
using System.Collections;

public class MainSceneManager : MonoSingleton<MainSceneManager>
{
    [SerializeField]
    public Transform SignalPrefab;

    //CurrentGameMode : normal, CallAdjuster{режим вызванного монтажника - автоустановка положений}, Win

	void Start ()
	{
	    InvokeRepeating("CreateSignal", 1f, 7f);
	}
	
	void Update ()
	{
	}

    private void CreateSignal()
    {
        Vector3 pos = ConnectorsManager.StartConnector.transform.position;
        var signalGO = (Instantiate(SignalPrefab, pos, new Quaternion(0,0,0,0)) as Transform);//.GetComponent<Signal>();
        var signal = signalGO.GetComponent<Signal>();
        signal.Init(ConnectorsManager.StartConnector.CurrentDirection);
    }

    public static void OnShapeRotateStart(Shape shape)
    {
        SignalsUtils.DestroySignalsInCell(shape.Xindex, shape.Yindex);
        ConnectorsManager.CheckAllConnections();
    }
}
