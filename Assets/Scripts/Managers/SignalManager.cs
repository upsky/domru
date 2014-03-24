using System.Linq;
using UnityEngine;
using System.Collections;
using Shapes;

public class SignalManager : RequiredMonoSingleton<SignalManager>
{
    [System.Serializable]
    private class SpawnItem
    {
        public Shape Shape =null;
        public Direction Side = Direction.None;
    }


    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private float _firstSpawnTime = 1f;

    [SerializeField]
    private float _repeatSpawnInerval = 7f;

    [SerializeField]
    private SpawnItem[] _spawnItems;

    [SerializeField]
    private bool _randomCloning = false;

    [SerializeField]
    private int _maxSignalsCount = 100;

    [SerializeField, ReadOnlyInInspector]
    private int _signalsCount = 0;

    public static bool IsAllowedCreateSignal
    {
        get
        {
            if (Instance == null)
                return false;
            return Instance._signalsCount < Instance._maxSignalsCount;
        }
    }

    /// <summary>
    /// При отключенной опции, клонирование сигнала будет выполняться всегда при ветвлении провода. При влюченной - выбор клонировать или нет выполняется случайно.
    /// </summary>
    public static bool IsRandomCloning
    {
        get { return Instance._randomCloning; }
    }


    private void Start()
    {
        InvokeRepeating("CreateSignal", _firstSpawnTime, _repeatSpawnInerval);
    }

    public static void OnCreateSignal()
    {
        if (Instance != null)
            Instance._signalsCount++;
    }

    public static void OnDestroySignal()
    {
        if (Instance != null)
            Instance._signalsCount--;
    }


    private void CreateSignal()
    {
        if (!IsAllowedCreateSignal)
            return;

        int index = Random.Range(0, _spawnItems.Length);

        var shape = _spawnItems[index].Shape;
        var direction = _spawnItems[index].Side;

        if (shape.GetPath(direction).Count == 0)
        {
            return;
        }
        var startPoint = shape.GetPath(direction).First();

        var signalGO = (Instantiate(_prefab, startPoint, new Quaternion(0, 0, 0, 0)) as GameObject);
        var signal = signalGO.GetComponent<Signal>();
        signal.Init(direction, _prefab);
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnItems == null || _spawnItems.Length==0)
            return;
        Gizmos.color = ColorUtils.CreateWithAlpha(Color.green, 0.5f);
        foreach (var item in _spawnItems)
        {
            if (item.Shape!=null)
                Gizmos.DrawSphere(item.Shape.transform.position, 0.3f);
        }
    }

}


