using UnityEngine;
using System.Collections;

public static class AstarPathUtils
{
    public static void CreateGraphIfItNull()
    {
        if (AstarPath.active == null || AstarPath.active.astarData == null || AstarPath.active.astarData.gridGraph == null)
            AstarPath.MenuScan();
    }
}
