using Shapes;
using UnityEngine;
using System.Collections;
using System.Linq;
public class Connector : MonoBehaviour
{
    //public Device

    public bool IsStartConnector;

    [ReadOnlyInInspector]
    public Direction CurrentDirection;


    [HideInInspector]
    public Shape NearestShape;

    private bool _isConnected;

    void Awake()
    {
        //автоустановка правильного значения CurrentDirection при старте игры
        CurrentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y).GetNext();//.GetNext() - т.к. модель повернута не так, как нужно
    }

    // Use this for initialization
    private void Start()
    {
        if (!IsStartConnector)
            renderer.material.color = Color.red;
        var node =  AstarPath.active.astarData.gridGraph.GetNearest(transform.position).node;

        var colladers = Physics.OverlapSphere(node.position.ToVector3(), 0.3f);
        foreach (var collader in colladers)
        {
            NearestShape = collader.GetComponent<Shape>();
            if (NearestShape!=null)
                break;
        }
        
        if (NearestShape==null)
            Debug.LogError("Shape not found");
    }

    public void SwitchToOn()
    {
        if (_isConnected)
            return;
        renderer.material.color = Color.green;
        _isConnected = true;
        
        //todo: умедомление MainSceneManager-а, а в нем уже вызов проверки на победу - проверка количества подключенных коннекторов.
    }

    public void SwitchToOff()
    {
        if (!_isConnected)
            return;
         renderer.material.color = Color.red;
        _isConnected = false;
    }

}
