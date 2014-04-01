﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shapes;
using UnityEngine;

public static class ShapesPathBaker
{
    private static readonly List<Shape> _traversedShapes = new List<Shape>();
    private static readonly List<Connector> _unTraversedConnectors = new List<Connector>();
    
    private static readonly List<ChainItem> _chainItems=new List<ChainItem>(); //для сохранения обхода

    public static void FindAndSavePath()
    {
        _traversedShapes.Clear();
        _chainItems.Clear();
        
        _unTraversedConnectors.AddRange(ConnectorsManager.TargetConnectors);

        TraverseRecursively(ConnectorsManager.StartConnector.NearestShape, _chainItems);

        ShapesSorter.SetChain(_chainItems);
    }

    private static bool TraverseRecursively(Shape shape, List<ChainItem> chain)
    {        
        _traversedShapes.Add(shape);
        bool chainItemHasConnector = false;

        ChainItem chainItem = new ChainItem();
        chainItem.Shape = shape;
        chainItem.TargetDirection = shape.CurrentDirection;
        chain.Add(chainItem);

        var connector = FindConnectorWithConnection(shape);
        if (connector != null && shape.HasConnection(connector.CurrentDirection))
        {
            //Debug.LogWarning(connector.name);
            chainItemHasConnector = true;
            _unTraversedConnectors.Remove(connector);
        }

        var neighbors = NodesGrid.FindConnectedNeighborShapes(shape);

        bool hasFirstChain = false;
        foreach (var neighbor in neighbors)
        {            
            if (!_traversedShapes.Contains(neighbor))
            {
                List<ChainItem> targetChain = chain;
                if (hasFirstChain)
                    targetChain = chainItem.childChain;

                //string shapeName = neighbor.Xindex + "," + neighbor.Yindex + "  type=" + neighbor.GetType().Name;
                //Debug.LogWarning(shapeName, neighbor);
                //neighbor.name = shapeName;

                bool nextChainItemHasConnector = TraverseRecursively(neighbor, targetChain);
                chainItemHasConnector |= nextChainItemHasConnector;
                hasFirstChain = true;
            }
        }

        if (!chainItemHasConnector)
        {
            //chainItem.Shape.name += "_RemovedChainItem";
            chain.Remove(chainItem);
        }

        return chainItemHasConnector;
    }

    private static Connector FindConnectorWithConnection(Shape shape)
    {
        return _unTraversedConnectors.FirstOrDefault(c => c.NearestShape == shape);
    }

}
