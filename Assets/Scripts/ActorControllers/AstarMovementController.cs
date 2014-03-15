using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Linq;

public class AstarMovementController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;

    private Seeker seeker;
    private Path _path;
    private int _currentWaypointIndex;
    private Vector2 _currentWaypoint;

    /// <summary>
    /// Максимальное расстояние от объекта до точки пути, к которой он движется, при достижении которого он может двигаться к следующей точке. Использовать только в FixedUpdate().
    /// </summary>
    private float NextWaypointDistance
    {
        get { return Time.fixedDeltaTime * _speed* 1.01f; }
    }


    // Use this for initialization
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, SceneContainers.SeekerTargets.GetChild(0).position, OnPathComplete);//SeekerTargets.GetComponentsInChildren<Transform>().First()
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

    private void DirectMove(Vector2 targetPoint)
    {
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

    private void OnClick()
    {
        Debug.LogWarning("CAT");
    }
}
