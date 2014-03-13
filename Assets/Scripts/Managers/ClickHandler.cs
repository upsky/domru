using UnityEngine;
using System.Collections;

public class ClickHandler : MonoSingleton<ClickHandler>
{

    //public static bool MouseOverGUI = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hit.transform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver); 
            }
        }
    }
}
