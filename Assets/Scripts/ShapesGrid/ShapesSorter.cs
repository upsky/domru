using System.Collections.Generic;

using UnityEngine;
using System.Collections;


public class ShapesSorter : MonoSingleton<ShapesSorter>
{
    [SerializeField]
    private List<ChainItem> _chainItems;

    /////<remarks> Только для редактора, т.к. некогда рефлексить</remarks>
    //public List<ChainItem> ChainItems
    //{
    //    get { return _chainItems; }
    //}

	void Start ()
	{ 
	}
    
    public static void StartSorting()
    {
        Instance.StartCoroutine(Instance.SortChainRecursively(Instance._chainItems));
	}

    private IEnumerator SortChainRecursively(List<ChainItem> chainItems)//string name, int level)
    {
        foreach (var item in chainItems)
        {
            if (item.Shape != null)
            {
                item.Shape.RotateToDirection(item.TargetDirection);
                //Debug.LogWarning(item.Shape.name, item.Shape);
                yield return new WaitForSeconds(0.05f);

                if (item.childChain != null)
                    StartCoroutine(SortChainRecursively(item.childChain));
            }
        }
    }

}
