using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PhysicsUtils
{
    public static T[] OverlapSphere<T>(Vector3 position, float radius) where T : Component
    {
        List<T> retList=new List<T>();
        var colladers = Physics.OverlapSphere(position, radius);
        foreach (var c in colladers)
        {
            var targetComponent = c.GetComponent<T>();
            if (targetComponent != null)
                retList.Add(targetComponent);
        }

        return retList.ToArray();
    }
}