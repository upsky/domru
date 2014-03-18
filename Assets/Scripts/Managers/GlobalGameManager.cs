using UnityEngine;
using System.Collections;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField]
    private Transform _signalPrefab;

    public static Transform SignalPrefab
    {
        get { return Instance._signalPrefab; }
    }

    void Start () 
    {
	    DontDestroyOnLoad(gameObject);
	}
}
