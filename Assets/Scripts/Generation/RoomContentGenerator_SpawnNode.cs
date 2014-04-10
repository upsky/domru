using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using Shapes;
using Random = UnityEngine.Random;

public partial class RoomContentGenerator
{
    private class SpawnNode
    {
        public bool IsConnectorNode;
        public bool IsDeviceNode;
        public bool IsCoverNode;
        public bool IsBusyNode;
        public bool IsEmpty
        {
            get { return !(IsConnectorNode || IsDeviceNode || IsCoverNode || IsBusyNode); }
        }
    
        public NodesGrid.Node GridNode;

        /// <summary>
        /// Направление, противоположное стене. Для угловых нод, всегда за данное направление отвечает NodesGrid.Node.Y
        /// </summary>
        public Direction Direction1 = Direction.None;

        /// <summary>
        /// Дополнительное направление, противоположное второй стене.(только для угловых нод) 
        /// </summary>
        public Direction Direction2 = Direction.None;

        public SpawnNode(NodesGrid.Node gridNode)
        {
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
    /// Наиболее удаленная нода от уже занятых
    /// </summary>
    /// <returns></returns>
    private SpawnNode GetFarEmptyNode()
    {
        //индексы, занятых нод
        var indexes = Enumerable.Range(0, _allNodes.Count)
                                .Where(i => !_allNodes[i].IsEmpty)
                                .ToList();

        //поиск индекса ноды, наиболее отдаленной от 2-х занятых нод 
        int maxDist = 0;
        int farIndex = 0;
        for (int i = 1; i < indexes.Count; i++)
        {
            if (indexes[i] - indexes[i - 1] > maxDist)
            {
                maxDist = indexes[i] - indexes[i - 1];
                farIndex = indexes[i-1] + maxDist/2;
            }
        }

        //сравнение найденной дистанции со значением дистанции между первой и последней незанятыми нодами.
        int lastDist = indexes[0] + 1 + (_allNodes.Count - 1) - indexes[indexes.Count - 1];
        if (lastDist > maxDist)
        {
            maxDist = lastDist;
            farIndex = indexes[indexes.Count - 1] + maxDist / 2;
        }


        var pos = _allNodes[farIndex].GridNode.Position;
        Debug.DrawLine(pos, pos + Vector3.up*4, Color.blue, 100f);
        //получить массив индексов из _allnodes, которые занятые
        //найди наибольшее значения между 2-мя индексами

        return _allNodes[farIndex];
    }

    /// <summary>
    /// Получает самую дальнюю от коннекторов ноду, являющуюся соседней текущей ноде.
    /// </summary>
    /// <remarks>Учтены не все баговые ситуации, т.к. применяется только для генерации конектора компа. 
    /// Т.к. расстояние между занятыми нодами довольно большое, то некоторые ошибочные ситуации не воспроизведутся</remarks>
    private SpawnNode GetFarthestFromConnectorstNeighborNode(SpawnNode node)
    {
        var index = _allNodes.FindIndex(c => c == node);

        int k1=1;
        for (int i = index+1; i < _allNodes.Count; i++)
        {
            if (_allNodes[i].IsConnectorNode)
                break;
            k1++;

            if (i == _allNodes.Count - 1) //если последний проход и не был найден коннектор, то прибавляем расстояние до стартового коннектора
            {
                if (_allNodes[0].IsConnectorNode)
                    k1++;
                else
                    k1 += 2;
            }
        }

        int k2 = 1;
        for (int i = index-1; i >= 0; i--)
        {
            if (_allNodes[i].IsConnectorNode)
                break;
            k2++;
        }

        int targetIndex;
        if (k1 >= k2)
        {
            targetIndex = index + 1;
        }
        else
        {
            targetIndex = index - 1;
        }

       return _allNodes[targetIndex];
    }

    private void RemoveNodeFromEmptyNodes(SpawnNode node, ref bool flagForChanging)
    {
        flagForChanging = true;
        _emptyNodes.Remove(node);
    }

    private void DebugEmptyNodes()
    {
        foreach (var node in _emptyNodes)
            Debug.DrawLine(node.GridNode.Position, node.GridNode.Position + Vector3.up*2, Color.red, 100f);
    }

    /// <summary>
    /// Добавление нод в список против часовой от ноды справа от двери [6,0]
    /// </summary>
    private void FillNodes()
    {
        //_allNodes.Add(new SpawnNode(NodesGrid.Grid[6, 0]));
        for (int i = 6; i >= 0; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 0]));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[0, i]));

        for (int i = 1; i < 7; i++)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[i, 6]));

        for (int i = 5; i >= 1; i--)
            _allNodes.Add(new SpawnNode(NodesGrid.Grid[6, i]));

        _emptyNodes.AddRange(_allNodes);
        RemoveNodeFromEmptyNodes(_allNodes[1], ref _allNodes[1].IsBusyNode);
    }
    
}
