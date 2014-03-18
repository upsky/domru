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


    [HideInInspector]
    public Shape NearestShape;

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
        var node = AstarPath.active.astarData.gridGraph.GetNearest(transform.position).node;

        NearestShape = PhysicsUtils.OverlapSphere<Shape>(node.position.ToVector3(), 0.3f).FirstOrDefault();

        if (NearestShape == null)
            Debug.LogError("Shape not found");
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
