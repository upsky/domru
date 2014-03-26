using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Shapes;

public static class ShapesGeberator
{
    public static void StartGeneration()
    {
        //;
        var astarNode = AstarPath.active.astarData.gridGraph.GetNearest(ConnectorsManager.StartConnector.transform.position).node;
        int x = Mathf.RoundToInt(astarNode.position.ToVector3().x);
        int y = Mathf.RoundToInt(astarNode.position.ToVector3().z);
        
        Debug.LogWarning(x + "," + y);
        var node = NodesGrid.Grid[x, y];
    }

    public static void GenerateRecursively(NodesGrid.Node node)//List<ChainItem> chainItems) //string name, int level)
    {
        if (!node.IsAvailable)
            return;

        node.SetShape(CreateRandomShape());//, x,y);
        //node.Shape.gets

        //foreach (var item in chainItems)
        //{
        //    if (item.Shape != null)
        //    {
        //        item.Shape.RotateToDirection(item.TargetDirection);
        //        //Debug.LogWarning(item.Shape.name, item.Shape);
        //       // yield return new WaitForSeconds(0.05f);

        //        //if (item.childChain != null)
        //         //   StartCoroutine(SortChainRecursively(item.childChain));
        //    }
        //}

    }

    public static Shape CreateRandomShape()
    {
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0:
                return new LineShape();
            case 1:
                return new CornerShape();
            default:
                return new TeeShape();
        }
    }

    /// <summary>
    /// Вращает targetShape, пока он не будет соединен с другим, если это возможно.
    /// </summary>
    public static void RotateToConnection(Shape shape, Shape targetShape)
    {
        for (int i = 0; i < 4; i++)
        {
            if (shape.HasConnection(targetShape))
                return;
            targetShape.FastRotate();
        }
        Debug.LogError("connection not found");
    }

    ////добавляет соединенный shape к shape в текущем node
    //public static Shape AddConnectedShape(NodesGrid.Node node)
    //{
    //    //CreateRandomShape();
    //}

}
