using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BtnToggleIcon : MonoBehaviour
{
    [SerializeField]
    private string _state1;

    [SerializeField]
    private string _state2;

    public string StateOn {
        get { return _state1; }
    }

    public string StateOff
    {
        get { return _state2; }
    }

    private void OnClick()
    {
        var sprite = GetComponent<UISprite>();

        if (sprite.spriteName == _state1)
            sprite.spriteName = _state2;
        else if (sprite.spriteName == _state2)
            sprite.spriteName = _state1;
        else
            Debug.LogError("undefined sprite");
    }
}
