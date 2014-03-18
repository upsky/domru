using System.Collections.Generic;
using System.Linq;
using Shapes;
using UnityEngine;
using System.Collections;

public class ConnectorsManager : RequiredMonoSingleton<ConnectorsManager>
{
    public static Connector StartConnector
    {
        get { return Instance._startConnector; }
    }

    /// <summary>
    /// Все коннекторы, которые ожидают прихода сигнала
    /// </summary>
    public static Connector[] TargetConnectors
    {
        get { return Instance._targetConnectors; }
    }

    private Connector _startConnector = null;
    private Connector[] _targetConnectors;
    private List<Connector> _unConnectedConnectors = new List<Connector>();

    private List<Shape> _traversedShapes = new List<Shape>();

    private void Start()
    {
        //Init connectors

        List<Connector> targetConnectors = new List<Connector>();
        foreach (Transform connector in SceneContainers.Connectors)
        {
            var c = connector.GetComponent<Connector>();
            if (c.IsStartConnector)
            {
                if (_startConnector == null)
                    _startConnector = c;
                else
                    Debug.LogError("Флаг `IsStartConnector=true` больше чем у одного коннектора в сцене");
            }
            else
            {
                targetConnectors.Add(c);
            }
        }

        _targetConnectors = targetConnectors.ToArray();

        if (_startConnector == null)
            Debug.LogError("_startConnector not found");
    }

    /// <summary>
    /// Проверяет, был ли подключен коннектор при последней проверке CheckAllConnections()
    /// </summary>
    public static bool IsConnectedAtLastChecking(Connector c)
    {
        return !Instance._unConnectedConnectors.Contains(c);
    }

    public static void CheckAllConnections()
    {
        Instance._unConnectedConnectors.Clear();
        Instance._unConnectedConnectors.AddRange(Instance._targetConnectors);

        //if Has Start Connection
        if (Instance._startConnector.NearestShape.HasConnection(Instance._startConnector.CurrentDirection))
        {
            Instance.CheckConnectRecursively(Instance._startConnector.NearestShape);
            Instance._traversedShapes.Clear();
        }

        foreach (var c in Instance._unConnectedConnectors)
            c.SwitchToOff();
    }

    private void CheckConnectRecursively(Shape shape)
    {
        _traversedShapes.Add(shape);

        var connector = FindConnectorWithConnection(shape);
        if (connector != null && shape.HasConnection(connector.CurrentDirection))
        {
            _unConnectedConnectors.Remove(connector);
        }

        var neighbors = ShapesGrid.FindConnectedNeighborShapes(shape);
        foreach (var neighbor in neighbors)
        {
            if (!_traversedShapes.Contains(neighbor))
                CheckConnectRecursively(neighbor);
        }
    }

    private Connector FindConnectorWithConnection(Shape shape)
    {
        return _unConnectedConnectors.FirstOrDefault(c => c.NearestShape == shape);
    }

}
