using System;
using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Linq;

public class AstarMovementController : MonoBehaviour, IPathFinderMovement
{
    [SerializeField]
    private float _speed = 1f;

    private Seeker _seeker;
    private Path _path;
    private int _currentWaypointIndex;
    private Vector2 _currentWaypoint;
    private Action _onPathTraversed;

    /// <summary>
    /// Максимальное расстояние от объекта до точки пути, к которой он движется, при достижении которого он может двигаться к следующей точке. Использовать только в FixedUpdate().
    /// </summary>
    private float NextWaypointDistance
    {
        get { return Time.fixedDeltaTime * _speed* 1.01f; }
    }


    // Use this for initialization
    private void Awake()
    {
        _seeker = GetComponent<Seeker>();        
    }

    private void FixedUpdate()
    {
        if (_path != null)
        {
            //проверка-достигнут ли конец пути
            //Vector2 currentWaypoint = _path.vectorPath[_currentWaypointIndex].xz();
            Vector2 endWaypoint = _path.vectorPath[_path.vectorPath.Count - 1].xz();
            if (Vector2.Distance(endWaypoint, transform.position.xz()) <= NextWaypointDistance)//_nextWaypointDistance)
            {
                //_performer.UnitAnimation.State = UnitAnimation.States.Idle;
                _path = null;
                _onPathTraversed();
                return;
            }

            //поворот и перемещение к текущей Waypoint
            UpdateCurrentWaypoint();
            Rotate(_currentWaypoint);
            Move(_currentWaypoint);            
        }
        //else  //если пути нет
        //    _performer.UnitAnimation.State = UnitAnimation.States.Idle;
    }

    public void StartMovement(Transform target, Action onMovementStart, Action onPathTraversed)
    {
        if (target == null)
        {
            Debug.LogWarning("target=null", this);
            return;
        }
        
        _onPathTraversed = onPathTraversed;
        _seeker.StartPath(transform.position, target.position, (p) =>
            {
                OnPathComplete(p);
                onMovementStart();
            });
    }

    public void CancelMovement()
    {
        _path = null;
    }

    private void UpdateCurrentWaypoint()
    {
        Vector2 currentWaypoint = _path.vectorPath[_currentWaypointIndex].xz();
        if (Vector2.Distance(currentWaypoint, transform.position.xz()) <= NextWaypointDistance) //проверка, разрешено ли двигаться к следущей точки пути, вместо текущей
        {
            if (_currentWaypointIndex < _path.vectorPath.Count - 1)
            {
                _currentWaypointIndex++;                
            }
        }
        _currentWaypoint = _path.vectorPath[_currentWaypointIndex].xz();
    }

    ///<param name="currentWaypoint">Ближайшая точка пути, к которой движется seeker</param>
    private void Move(Vector2 currentWaypoint)
    {
        Vector3 currentPos = transform.position;
        Vector3 dir = (currentWaypoint.ToVector3(currentPos.y) - currentPos).normalized;
        if (dir != Vector3.zero)
        {
            transform.position += (dir * Time.fixedDeltaTime * _speed);
           // UnitAnimation.State = UnitAnimation.States.Run;
        }
    }

    private void Rotate(Vector2 currentWaypoint)
    {
        Vector3 targetPos = currentWaypoint.ToVector3(transform.position.y);
        transform.LookAt(targetPos);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypointIndex = 1;
        }
        else
        {
            _path = null;
           // CompleteTask();
        }
    }
}
