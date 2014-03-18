using System.Linq;
using UnityEngine;
using System.Collections;
using Shapes;

public class SignalManager : RequiredMonoSingleton<SignalManager>
{
    [System.Serializable]
    private class SpawnItem
    {
        public Shape Shape;
        public Direction Side;
    }


    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private float _firstSpawnTime = 1f;

    [SerializeField]
    private float _repeatSpawnInerval = 7f;

    [SerializeField]
    private SpawnItem[] _spawnItems;

    void Start()
    {
        InvokeRepeating("CreateSignal", _firstSpawnTime, _repeatSpawnInerval);
    }

    private void CreateSignal()
    {
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


