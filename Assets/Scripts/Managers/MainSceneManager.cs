using UnityEngine;
using System.Collections;

public class MainSceneManager : MonoSingleton<MainSceneManager>
{
    [SerializeField]
    public Transform SignalPrefab;

    //CurrentGameMode : normal, CallAdjuster{режим вызванного монтажника - автоустановка положений}, Win

	// Use this for initialization
	void Start ()
	{
	    InvokeRepeating("CreateSignal", 1f, 7f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateSignal()
    {
        Vector3 pos = ShapesGrid.Instance.StartConnector.transform.position;
        var signalGO = (Instantiate(SignalPrefab, pos, new Quaternion(0,0,0,0)) as Transform);//.GetComponent<Signal>();
        var signal = signalGO.GetComponent<Signal>();
        signal.Init(ShapesGrid.Instance.StartConnector.CurrentDirection);
    }
}
