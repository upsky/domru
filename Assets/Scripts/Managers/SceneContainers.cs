using UnityEngine;
using System.Collections;

public class SceneContainers: MonoSingleton<SceneContainers>
{
    [SerializeField]
    private Transform _roomContent;

    [SerializeField]
    private Transform _shapes;

    [SerializeField]
    private Transform _seekerTargets;

    [SerializeField]
    private Transform _connectors;

    [SerializeField]
    private Transform _signalsSpawners;
    
    public static Transform RoomContent
    {
        get { return Instance._roomContent; }
    }

    public static Transform SeekerTargets
    {
        get { return Instance._seekerTargets; }
    }

    public static Transform Shapes
    {
        get { return Instance._shapes; }
    }

    public static Transform Connectors
    {
        get { return Instance._connectors; }
    }

    public static Transform SignalsSpawners
    {
        get { return Instance._signalsSpawners; }
    }
}
