
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[ExecuteInEditMode]
public class GridAligner : MonoBehaviour
{
#if UNITY_EDITOR
    #region Inspector variables
    //обзначения имен в этом регионе:
    //Containers - список родителей, прямые дети которых будут выравниваться.
    //Objects - спискок конкретных объектов, которые будут выравниваться.
    [SerializeField]
    private AstarPath _astarPath;

    [SerializeField]
    private float _yPos=0.3f;

    [SerializeField]
    private List<Transform> ContainersForAligningByXYZ;

    [SerializeField]
    private List<Transform> ObjectsForAligningByXYZ;

    [SerializeField]
    private List<Transform> ContainersForAligningByXZ;

    [SerializeField]
    private List<Transform> ObjectsForAligningByXZ;
    #endregion

    private void Start()
    {
    }

    private void Update()
    {
        if (Application.isPlaying || _astarPath==null)
            return;

        AstarPathUtils.CreateGraphIfItNull(_astarPath);

        AligningPositionByXYZ(GetListForAligning(ContainersForAligningByXYZ, ObjectsForAligningByXYZ));
        AligningPositionByXZ(GetListForAligning(ContainersForAligningByXZ, ObjectsForAligningByXZ));
    }

    private IEnumerable<Transform> GetListForAligning(IEnumerable<Transform> Containers, IEnumerable<Transform> concreteObjects)
    {
        List<Transform> allSelectedChilds = new List<Transform>();

        foreach (Transform child in Containers)
            allSelectedChilds.AddRange(child.Cast<Transform>());

        allSelectedChilds.AddRange(concreteObjects);
        return allSelectedChilds;
    }

    private void AligningPositionByXYZ(IEnumerable<Transform> transforms)
    {
        var gridGraph = AstarPath.active.astarData.gridGraph;
        foreach (var tr in transforms)
        {
            Vector3 newPos = gridGraph.GetNearest(tr.position).node.position.ToVector3();
            newPos.y += _yPos;
            tr.position = newPos;
        }
    }

    private void AligningPositionByXZ(IEnumerable<Transform> transforms)
    {
        var gridGraph = AstarPath.active.astarData.gridGraph;
        foreach (var tr in transforms)
        {
            Vector3 newPos = gridGraph.GetNearest(tr.position).node.position.ToVector3();
            newPos.y = tr.position.y;
            tr.position = newPos;
        }
    }
#endif
}
