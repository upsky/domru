using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.1f;

    private const float _posY = 10.32f;

    private Direction _prevOutDirection;

   
    private Shape _prevShape;
    private Shape _currentShape;
    private int _currentWaypointIndex = 0;
    private Vector3 _currentWaypoint;
    private List<Vector3> _path;

    private bool _isClonedInCurrentShape;

    /// <summary>
    /// Максимальное расстояние от объекта до точки пути, к которой он движется, при достижении которого он может двигаться к следующей точке.
    /// </summary>
    private float NextWaypointDistance
    {
        get { return Time.fixedDeltaTime * _speed * 1.01f; }
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
        //Debug.LogWarning(x+"_"+y);
    }


    private void TryClone()
    {
        TeeShape teeShape = _currentShape as TeeShape;
        if (teeShape != null && _currentWaypointIndex == 2 && _path.Count == 3)// у клонированного сигнала путь содержит только 2 точки, поэтому он не пройдет проверку:  _path.Count == 3
        {            
            var signalGO = (Instantiate(MainSceneManager.Instance.SignalPrefab, _currentWaypoint, new Quaternion(0, 0, 0, 0)) as Transform);//.GetComponent<Signal>();
            var signal = signalGO.GetComponent<Signal>();

            signal._path = teeShape.GetSecondPath(_prevOutDirection);
            var dir = teeShape.GetSecondOutDirection(_prevOutDirection);
            
            signal._prevOutDirection = dir;
            signal.transform.SetY(_posY);
            signal._isClonedInCurrentShape = true;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //TestOuts();
        //TestPath();
        //return;

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

            _prevShape = _currentShape;
            _currentShape = ShapesGrid.GetNextShape(_currentShape, _prevOutDirection);

            //if (_currentShape!=null)
            //    Debug.LogWarning("_prevOutDirection=" + _prevOutDirection + " HasConnection=" + _currentShape.HasConnection(_prevOutDirection), _currentShape);
            if (_currentShape == null || !_currentShape.HasConnection(_prevOutDirection))
            {
                Debug.LogWarning("DestroySignal");
                DestroySignal();
                return;
            }
            _path = _currentShape.GetPath(_prevOutDirection);
            _isClonedInCurrentShape = false;
        }


        //поворот и перемещение к текущей Waypoint
        UpdateCurrentWaypoint();
        Rotate(_currentWaypoint);
        Move(_currentWaypoint);  
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
        Vector3 endWaypoint = _path[_path.Count - 1];
        return Vector3.Distance(endWaypoint, transform.position) <= NextWaypointDistance;
    }



    /*void OnComeToConnector(Connector connector)
    {
       CheckConnection(connector)//проверка конкретного соединения
    }
    */

    private void DestroySignal()
    {
        //если уничтожение сигнала произошло в shape, на который ссылается коннектор и направление выхода сигнала совпадает с противоположным направлением коннектора, то включение коннектора.

        //вроде уведомлять никого не нужно при уничтожении сигнала. Хотя менеджер нужно, для проверки коннекторов или сами коннекторы
        Destroy(gameObject);
    }






    private void TestOuts()
    {
        //int x = 2, y = 2; //Line
        //int x = 2, y = 3; //Corner
        int x = 6, y = 0; //Tee
        var outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Up);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType()+" InDir=Up: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Right);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Right: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Down);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Down: outDir=" + outDir, ShapesGrid.Grid[y, x]);

        outDir = ShapesGrid.Grid[y, x].GetOutDirection(Direction.Left);
        Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Left: outDir=" + outDir, ShapesGrid.Grid[y, x]);
    }
    private void TestPath()
    {
        //int x = 2, y = 2; //Line
        int x = 2, y = 3; //Corner
        //int x = 6, y = 0; //Tee
        var path = ShapesGrid.Grid[y, x].TestPath(Direction.Up);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Up: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Right);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Right: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Down);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Down: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");

        path = ShapesGrid.Grid[y, x].TestPath(Direction.Left);
        foreach (var pointName in path)
            Debug.LogWarning(ShapesGrid.Grid[y, x].GetType() + " InDir=Left: " + pointName, ShapesGrid.Grid[y, x]);
        Debug.LogWarning("--------------------------------");
    }
}

