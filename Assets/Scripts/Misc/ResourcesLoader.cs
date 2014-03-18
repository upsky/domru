using UnityEngine;
using System.Collections;

public class ResourcesLoader : MonoSingleton<ResourcesLoader>
{
    private const string _signalPrefabPath = "Prefabs/Signals/OscillographSpark";
    private GameObject _signalPrefab;

    public static GameObject SignalPrefab
    {
        get { return Instance._signalPrefab; }
    }

    protected override void Awake()
    {
        base.Awake();
        var _prefab = (GameObject)Resources.Load(_signalPrefabPath);

        if (_prefab == null)
        {
            Debug.LogError("Cannot Load Prefab " + _signalPrefabPath);
            return;
        }
        _signalPrefab = _prefab;
    }
}
