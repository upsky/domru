using System.Linq;
using UnityEngine;
using System.Collections;
using Shapes;

public class SignalSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private Direction _direction = Direction.Up;

    [SerializeField]
    private float _firstSpawnTime = 1f;

    [SerializeField]
    private float _repeatSpawnInerval = 7f;

    void Start()
    {
        InvokeRepeating("CreateSignal", _firstSpawnTime, _repeatSpawnInerval);
    }

    private void CreateSignal()
    {
        //var dirOffset = DirectionUtils.DirectionToVector3(_direction);//смещение в сторону _direction на 1 клетку 
        //Vector3 pos = _targetShape.transform.position + dirOffset;
        var node = AstarPath.active.astarData.gridGraph.GetNearest(transform.position).node;
        var nearestShape = PhysicsUtils.OverlapSphere<Shape>(node.position.ToVector3(), 0.3f).FirstOrDefault();
        if (nearestShape == null)
        {
            Debug.LogError("nearestShape not found",this);
            return;
        }

        if (nearestShape.GetPath(_direction).Count==0)
        {
            return;
        }
        var startPoint = nearestShape.GetPath(_direction).First();

        var signalGO = (Instantiate(_prefab, startPoint, new Quaternion(0, 0, 0, 0)) as GameObject);
        var signal = signalGO.GetComponent<Signal>();
        signal.Init(_direction, _prefab);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = ColorUtils.CreateWithAlpha(Color.green, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
