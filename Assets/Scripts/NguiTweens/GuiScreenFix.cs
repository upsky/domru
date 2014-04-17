using UnityEngine;
using System.Collections;

public class GuiScreenFix : MonoBehaviour
{
    private UIRect _uiItem;

	void Start ()
	{
        _uiItem = this.GetSafeComponent<UIRect>();

        //float h = Screen.currentResolution.height;
        //float w = Screen.currentResolution.width;
        float w = Screen.width ;
        float h = Screen.height;

	    float res = h/w;
        Debug.LogWarning(res);

	    if (res > 1.55) //16:10
	    {
            Camera.main.transform.SetY(22.7f);
            _uiItem.bottomAnchor.Set(-136f, 0f);
	    }

        if (res > 1.45 && res < 1.55) //3:2
        {


        }

        if (res < 1.45) //4:3
        {
            Camera.main.transform.SetY(21.08f);
            _uiItem.bottomAnchor.Set(-90f, 0f);
        }
        //Debug.LogWarning("16:10="+ 16f/10f);
        //Debug.LogWarning("3:2=" + 3f / 2f);
        //Debug.LogWarning("4:3=" + 4f / 3f);
	    //_uiItem.topAnchor
	}

   
}
