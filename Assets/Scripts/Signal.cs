using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private const float _posY = 10.32f;

    private Direction _prevOutDirection;

    private Shape _currentShape;
    private int _currentWaypointIndex = 0;
    private Vector3 _currentWaypoint;
    private List<Vector3> _path;

    /// <summary>
    /// Сигнал-предок, который клонировал текущий сигнал.
    /// </summary>
    private Signal _parentSignal;

    private bool _isClonedInCurrentShape;

    /// <summary>
    /// Максимальное расстояние от объекта до точки пути, к которой он движется, при достижении которого он может двигаться к следующей точке.
    /// </summary>
    private float NextWaypointDistance
    {
        get { return Time.fixedDeltaTime * _speed * 1.01f; }
    }

    private void Awake()
    {
        if (ShapesGrid.Grid == null)
        {
            Debug.LogError("ShapesGrid.Grid is not initialized");
            return;
        }
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.z);
        _currentShape = ShapesGrid.Grid[y, x];
    }

    /// <summary>
    /// Инициализация для использования вне скрипта.
    /// </summary>
    /// <param name="dir"></param>
    public void Init(Direction dir)
    {
        _prevOutDirection = dir;
        transform.SetY(_posY);

        _path = _currentShape.GetPath(_prevOutDirection);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_path == null)
        {
            return;
        }

        if (PathIsTraversed())
        {
            _path = null;
            _currentWaypointIndex = 0;

            if (!_isClonedInCurrentShape)
                _prevOutDirection = _currentShape.GetOutDirection(_prevOutDirection);//получение направления выхода из текущей shape

            _currentShape = ShapesGrid.GetNextShape(_currentShape, _prevOutDirection);
            if (_currentShape == null || !_currentShape.HasConnection(_prevOutDirection))
            {
                DestroySelf();
                return;
            }

            _path = _currentShape.GetPath(_prevOutDirection);
            _isClonedInCurrentShape = false;
            _parentSignal = null;
        }

        UpdateCurrentWaypoint();
        Rotate(_currentWaypoint);
        Move(_currentWaypoint);  
    }

    private void OnTriggerEnter(Collider c)
    {
        var signal2 = c.GetComponent<Signal>();
        if (signal2 != null && signal2._parentSignal != this && _parentSignal != signal2)
        {
            DestroySelf();
        }
    }

    private void TryClone()
    {
        TeeShape teeShape = _currentShape as TeeShape;
        if (teeShape != null && _currentWaypointIndex == 2 && _path.Count == 3)// у клонированного сигнала путь содержит только 2 точки, поэтому он не пройдет проверку:  _path.Count == 3
        {
            var signalGO = (Instantiate(MainSceneManager.Instance.SignalPrefab, _currentWaypoint, new Quaternion(0, 0, 0, 0)) as Transform);
            var signal = signalGO.GetComponent<Signal>();

            signal._path = teeShape.GetSecondPath(_prevOutDirection);
            var dir = teeShape.GetSecondOutDirection(_prevOutDirection);

            signal._prevOutDirection = dir;
            signal.transform.SetY(_posY);
            signal._isClonedInCurrentShape = true;
            signal._parentSignal = this;
        }
    }

    private void UpdateCurrentWaypoint()//Action onWaypointIndexChange)
    {
        Vector3 currentWaypoint = _path[_currentWaypointIndex];
        if (Vector3.Distance(currentWaypoint, transform.position) <= NextWaypointDistance) //проверка, разрешено ли двигаться к следущей точки пути, вместо текущей
        {
            if (_currentWaypointIndex < _path.Count - 1)
            {
                _currentWaypointIndex++;
                //Debug.Break();
                TryClone();
            }
        }
        _currentWaypoint = _path[_currentWaypointIndex];
    }

    ///<param name="currentWaypoint">Ближайшая точка пути, к которой движется seeker</param>
    private void Move(Vector3 currentWaypoint)
    {
        Vector3 currentPos = transform.position;
        Vector3 dir = (currentWaypoint - currentPos).normalized;
        if (dir != Vector3.zero)
        {
            transform.position += (dir * Time.fixedDeltaTime * _speed);
        }
    }

    private void Rotate(Vector3 currentWaypoint)
    {
        Vector3 targetPos = currentWaypoint;
        transform.LookAt(targetPos);
    }

    private bool PathIsTraversed()
    {
        if (_path.Count == 0)
            return true;
        Vector3 endWaypoint = _path[_path.Count - 1];
        return Vector3.Distance(endWaypoint, transform.position) <= NextWaypointDistance;
    }



    /*void OnComeToConnector(Connector connector)
    {
       CheckConnection(connector)//проверка конкретного соединения
    }
    */

    private void DestroySelf()
    {
        //если уничтожение сигнала произошло в shape, на который ссылается коннектор и направление выхода сигнала совпадает с противоположным направлением коннектора, то включение коннектора.

        //вроде уведомлять никого не нужно при уничтожении сигнала. Хотя менеджер нужно, для проверки коннекторов или сами коннекторы
        Destroy(gameObject);
    }
}

