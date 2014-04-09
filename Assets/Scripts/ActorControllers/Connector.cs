using System;
using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class Connector : MonoBehaviour
{
    public bool IsStartConnector;

    [SerializeField, ReadOnlyInInspector]
    private Device _device;

    [ReadOnlyInInspector]
    public Direction CurrentDirection;

    private NodesGrid.Node _nearestNode;

    public NodesGrid.Node NearestNode
    {
        get
        {
            if (_nearestNode != null)
                return _nearestNode;

            var node = AstarPath.active.astarData.gridGraph.GetNearest(transform.position).node;
            VectorInt2 nodeIndex = node.position.ToVector3();

            _nearestNode = NodesGrid.Grid[nodeIndex.x, nodeIndex.y];
            return _nearestNode;
        }
    }


    public bool IsConnected { get; private set; }

    private void Awake()
    {
        //автоустановка правильного значения CurrentDirection при старте игры
        CurrentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y);//.GetNext(); //.GetNext() - т.к. модель повернута не так, как нужно
    }

    // Use this for initialization
    private void Start()
    {
        if (!Application.isPlaying)
            return;

        if (!IsStartConnector)
            renderer.material.color = Color.red;

        EventMessenger.Subscribe(GameEvent.StartGameProcess, this, FindNearestDevice);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying)
            return;
        //выранивания до 0.5
        float x = (float) Math.Ceiling(transform.position.x*2)/2;
        float z = (float) Math.Ceiling(transform.position.z*2)/2;
        transform.SetX(x);
        transform.SetZ(z);
    }
#endif

    private void OnTriggerEnter(Collider c) //проверка, пришел ли сигнал
    {
        if (IsStartConnector)
            return;

        ConnectorsManager.CheckAllConnections();
        if (ConnectorsManager.IsConnectedAtLastChecking(this))
            SwitchToOn();
    }


    private void SwitchToOn()
    {
        if (IsConnected)
            return;
        renderer.material.color = Color.green;
        IsConnected = true;

        if (_device==null)
            Debug.LogWarning("device not found",this);
        else
            _device.SwitchToOn();

        EventMessenger.SendMessage(GameEvent.ConnetorSwitchToOn, this);
    }

    public void SwitchToOff()
    {
        if (!IsConnected)
            return;
        renderer.material.color = Color.red;
        IsConnected = false;
        if (_device == null)
            Debug.LogWarning("device not found", this);
        else
            _device.SwitchToOff();
    }

    private void FindNearestDevice()
    {
        _device = TargetFindingMethods.FindNearestTarget<Device>(transform.position, 100f, int.MaxValue);
        if (_device == null)
            Debug.LogWarning("device not found", this);
    }

}
