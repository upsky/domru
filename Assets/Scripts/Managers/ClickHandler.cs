﻿using System;
using UnityEngine;
using System.Collections;

public class ClickHandler : RequiredMonoSingleton<ClickHandler>
{
    private LayerMask _clickableMmask = int.MaxValue;

    private void Start()
    {
       _clickableMmask = _clickableMmask.RemoveFromMask(Consts.Layers.Signals);
    }

    //public static bool MouseOverGUI = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickableMmask))
            {
                //Debug.LogWarning("OnClick_" + hit.transform.name);
                hit.transform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver); 
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //    ShapesGenerator.Generate();
    }
}
