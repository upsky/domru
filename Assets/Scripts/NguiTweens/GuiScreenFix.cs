﻿using UnityEngine;
using System.Collections;

public class GuiScreenFix : MonoBehaviour
{
    private UIRect _uiItem;

	void Start ()
	{
        _uiItem = this.GetSafeComponent<UIRect>();

        float w = Screen.width ;
        float h = Screen.height;

	    float res = h/w;
        //Debug.LogWarning(res);

	    if (res > 1.55) //16:10
	    {
           
            //_uiItem.bottomAnchor.Set(-136f, 0f);
	    }

        if (res > 1.45 && res < 1.55) //3:2
        {


        }

        if (res < 1.45) //4:3
        {

            //_uiItem.bottomAnchor.Set(-90f, 0f);
        }
	}

   
}