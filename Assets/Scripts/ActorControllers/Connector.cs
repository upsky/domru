using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Connector : MonoBehaviour
{
    public bool IsStartConnector;

    [SerializeField]
    private Device _device;

    [ReadOnlyInInspector]
    public Direction CurrentDirection;

    public Shape _nearestShape;

    public Shape NearestShape {
        get
        {
            if (_nearestShape != null)
                return _nearestShape;

            var node = AstarPath.active.astarData.gridGraph.GetNearest(transform.position).node;
            _nearestShape = PhysicsUtils.OverlapSphere<Shape>(node.position.ToVector3(), 0.3f).FirstOrDefault();

            if (_nearestShape == null)
                Debug.LogError("Shape not found", this);
            return _nearestShape;
        }
    }

    public bool IsConnected { get; private set; }

    private void Awake()
    {
        //автоустановка правильного значения CurrentDirection при старте игры
        CurrentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y).GetNext(); //.GetNext() - т.к. модель повернута не так, как нужно
    }

    // Use this for initialization
    private void Start()
    {
        if (!IsStartConnector)
            renderer.material.color = Color.red;
    }

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
        MainSceneManager.OnConnetorSwitchToOn();
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

}
