using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BtnSoundState : MonoBehaviour
{
    private BtnToggleIcon _btnToggleIcon;

    private UISprite _sprite;


    private void Start()
    {
        _btnToggleIcon = GetComponent<BtnToggleIcon>();
        _sprite = GetComponent<UISprite>();

        int disableSound = PlayerPrefs.GetInt("DisableSound");//при 0 - звук есть, при 1 -нету
        if (disableSound == 1)
        {  
            _sprite.spriteName = _btnToggleIcon.StateOff;
        }
    }

    private void OnClick()
    {
        int disableSound = PlayerPrefs.GetInt("DisableSound");//при 0 - звука есть, при 1 -нету
        if (disableSound == 0)
        {
            //отключить звук
            SoundManager.SetSound(false);
            PlayerPrefs.SetInt("DisableSound", 1);
        }
        else
        {
            //включить звук
            SoundManager.SetSound(true);
            PlayerPrefs.SetInt("DisableSound", 0);
        }
    }
}
