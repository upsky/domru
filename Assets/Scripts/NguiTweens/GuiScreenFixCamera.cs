using UnityEngine;
using System.Collections;

public class GuiScreenFixCamera : MonoBehaviour
{
    void Awake()
	{
        float w = Screen.width ;
        float h = Screen.height;

	    float res = h/w;
        //Debug.LogWarning(res);

	    if (res > 1.55) //16:10
	    {
            camera.transform.SetY(22.7f+0.2f);
	    }

        if (res > 1.45 && res < 1.55) //3:2
        {
            camera.transform.SetY(22.08f);//+0.2f);
        }

        if (res < 1.45) //4:3
        {
            camera.transform.SetY(21.08f);
        }
        //Debug.LogWarning("16:10="+ 16f/10f);
        //Debug.LogWarning("3:2=" + 3f / 2f);
        //Debug.LogWarning("4:3=" + 4f / 3f);
	    //_uiItem.topAnchor
	}

   
}
