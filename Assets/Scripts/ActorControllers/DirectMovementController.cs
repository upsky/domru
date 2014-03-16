using System;
using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Linq;

public class DirectMovementController : MonoBehaviour, ISimpleMovement
{
    [SerializeField]
    private float _speed = 1f;

    private Action _onPathTraversed;

    private Transform _target;

    /// <summary>
    /// Максимальное расстояние от объекта до точки пути, к которой он движется, при достижении которого он может двигаться к следующей точке. Использовать только в FixedUpdate().
    /// </summary>
    private float NextWaypointDistance
    {
        get { return Time.fixedDeltaTime * _speed* 1.01f; }
    }

    private void Awake()
    {     
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            //проверка-достигнут ли конец пути
            if (Vector2.Distance(_target.position.xz(), transform.position.xz()) <= NextWaypointDistance)
            {
                _target = null;
                _onPathTraversed();
                return;
            }
            Rotate(_target.position.xz());
            Move(_target.position.xz());            
        }
    }

    public void StartMovement(Transform target, Action onPathTraversed)
    {
        if (target == null)
        {
            Debug.LogWarning("target=null", this);
            return;
        }

        _target = target;
        _onPathTraversed = onPathTraversed;
    }

    ///<param name="currentWaypoint">Ближайшая точка пути, к которой движется seeker</param>
    private void Move(Vector2 currentWaypoint)
    {
        Vector3 currentPos = transform.position;
        Vector3 dir = (currentWaypoint.ToVector3(currentPos.y) - currentPos).normalized;
        if (dir != Vector3.zero)
        {
            transform.position += (dir * Time.fixedDeltaTime * _speed);
        }
    }

    private void Rotate(Vector2 currentWaypoint)
    {
        Vector3 targetPos = currentWaypoint.ToVector3(transform.position.y);
        transform.LookAt(targetPos);
    }
}

