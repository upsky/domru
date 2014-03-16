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

        var colladers = Physics.OverlapSphere(node.position.ToVector3(), 0.3f);
        foreach (var collader in colladers)
        {
            NearestShape = collader.GetComponent<Shape>();
            if (NearestShape != null)
                break;
        }

        if (NearestShape == null)
            Debug.LogError("Shape not found");
    }

    private void OnTriggerEnter(Collider c)
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

        _device.SwitchToOn();
        MainSceneManager.OnConnetorSwitchToOn();
    }

    public void SwitchToOff()
    {
        if (!IsConnected)
            return;
        renderer.material.color = Color.red;
        IsConnected = false;
        _device.SwitchToOff();
    }

}
