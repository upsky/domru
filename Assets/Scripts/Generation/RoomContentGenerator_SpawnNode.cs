using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using Shapes;
using Random = UnityEngine.Random;

public partial class RoomContentGenerator
{
    private enum SpawnNodeType
    {
        Empty,
        Connector,
        Device,
        Cover,
        Busy
        //если будут нужны ноды с двумя типами, то просто создать типы анагочные этому - DeviceAndConnector
    }

    //todo object.IN()


    private class SpawnNode
    {
        /// <summary>
        /// Индекс в массиве _allNodes
        /// </summary>
        public int Index { get; private set; }

        public SpawnNodeType NodeType = SpawnNodeType.Empty;
    
        public NodesGrid.Node GridNode;

        /// <summary>
        /// Направление, противоположное стене. Для угловых нод, всегда за данное направление отвечает NodesGrid.Node.Y
        /// </summary>
        public Direction Direction1 = Direction.None;

        /// <summary>
        /// Дополнительное направление, противоположное второй стене.(только для угловых нод) 
        /// </summary>
        public Direction Direction2 = Direction.None;

        public SpawnNode(NodesGrid.Node gridNode, int index)
        {
            Index = index;
            GridNode = gridNode;

            if (gridNode.Y == 0)
                Direction1 = Direction.Up;
            else if (gridNode.Y == 6)
                Direction1 = Direction.Down;
            else if (gridNode.X == 0)
                Direction1 = Direction.Right;
            else
                Direction1 = Direction.Left;

            if (gridNode.Y == 0 || gridNode.Y == 6)
            {
                if (gridNode.X == 0)
                    Direction2 = Direction.Right;
                else if (gridNode.X == 6)
                    Direction2 = Direction.Left;
            }
        }
    }




    /// <summary>
    /// Наиболее удаленная свободная нода от коннекторов
    /// </summary>
    private SpawnNode GetFarEmptyNodeFromConnectors()//(params SpawnNodeType[] types)
    {
        //индексы, нод с коннекторами
        var indexes = _allNodes.Where(n => n.NodeType==SpawnNodeType.Connector).Select(n => n.Index).ToList();

        int index = _allNodes.First(n => n.NodeType == SpawnNodeType.Connector).Index;
        indexes.Add(_allNodes.Count + index);

        //поиск индекса ноды, наиболее отдаленной от 2-х нод с коннекторами
        int maxDist = 0;
        int farIndex = 0;
        for (int i = 1; i < indexes.Count; i++)
        {
            if (indexes[i] - indexes[i - 1] > maxDist)
            {
                maxDist = indexes[i] - indexes[i - 1];
                farIndex = indexes[i - 1] + maxDist / 2;
            }
        }

        //var pos = _allNodes[farIndex].GridNode.Position;
        //Debug.DrawLine(pos, pos + Vector3.up * 4, Color.green, 100f);

        if (_allNodes[farIndex].NodeType != SpawnNodeType.Empty)
            return GetNearEmptyNode(_allNodes[farIndex]);
        return _allNodes[farIndex];
    }

     private SpawnNode GetNearEmptyNode(SpawnNode node)
    {
        var nextItems = ListUtils.CreateListFrom(node.Index, _allNodes);
        var prevItems = ListUtils.CreateReversedListFrom(node.Index, _allNodes);

        for (int i = 0; i < nextItems.Count; i++)
        {
            if (nextItems[i].NodeType==SpawnNodeType.Empty)
                return nextItems[i];
            if (prevItems[i].NodeType == SpawnNodeType.Empty)
                return prevItems[i];
        }
        Debug.LogError("Empty node not found");
        return null;
     }


    /// <summary>
    /// Получает самую дальнюю от коннекторов ноду, являющуюся соседней текущей ноде.
    /// </summary>
    /// <remarks>Учтены не все баговые ситуации, т.к. применяется только для генерации конектора компа. 
    /// Т.к. расстояние между занятыми нодами довольно большое, то некоторые ошибочные ситуации не воспроизведутся</remarks>
    private SpawnNode GetFarthestFromConnectorstNeighborNode(SpawnNode node)
    {
        var nextItems = ListUtils.CreateListFrom(node.Index, _allNodes);
        var prevItems = ListUtils.CreateReversedListFrom(node.Index, _allNodes);

        //если соседняя нода занята, то вернуть ноду с другой стороны
        if (nextItems[0].NodeType!= SpawnNodeType.Empty)
            return prevItems[0];
        if (prevItems[0].NodeType != SpawnNodeType.Empty)
            return nextItems[0];

        for (int i = 0; i < nextItems.Count; i++)
        {
            if (nextItems[i].NodeType != SpawnNodeType.Connector)
                return prevItems[0];
            if (prevItems[i].NodeType != SpawnNodeType.Connector)
                return nextItems[0];
        }
        Debug.LogError("node with connector not found");
        return null;
    }








    private void RemoveNodeFromEmptyNodes(SpawnNode node, SpawnNodeType newType)
    {
        node.NodeType = newType;
        _emptyNodes.Remove(node);
    }

    private void DebugEmptyNodes()
    {
        foreach (var node in _allNodes) //_emptyNodes)
        {
            if (node.NodeType != SpawnNodeType.Empty)
                Debug.DrawLine(node.GridNode.Position, node.GridNode.Position + Vector3.up*2, Color.red, 100f);
        }
    }

    /// <summary>
    /// Добавление нод в список против часовой от ноды справа от двери [6,0]
    /// </summary>
    private void FillNodes()
    {
        int index = 0;
        //_allNodes.Add(new SpawnNode(NodesGrid.Grid[6, 0]));
        for (int i = 6; i >= 0; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 0], index++));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[0, i], index++));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 6], index++));

        for (int i = 5; i >= 1; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[6, i], index++));

        _emptyNodes.AddRange(_allNodes);
        RemoveNodeFromEmptyNodes(_allNodes[1], SpawnNodeType.Busy);
    }
    
}
