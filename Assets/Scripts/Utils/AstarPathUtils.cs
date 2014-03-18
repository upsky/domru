using UnityEngine;
using System.Collections;

public static class AstarPathUtils
{ 
    #if UNITY_EDITOR
    /// <summary>
    /// Editor only!!!
    /// </summary>
    public static void CreateGraphIfItNull(AstarPath path)
    {
        if (path == null)
        {
            Debug.LogWarning("AstarPath not found");
            return;
        }
        if (path.astarData == null || path.astarData.gridGraph == null)
            AstarPath.MenuScan();
    }
     #endif
}
