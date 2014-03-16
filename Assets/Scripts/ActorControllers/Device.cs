using UnityEngine;
using System.Collections;

public class Device : MonoBehaviour
{
    private float timeForOFF;
    private SpriteChanger _spriteChanger;

    private void Start()
    {
        _spriteChanger = GetComponent<SpriteChanger>();        
        enabled = false;
    }

    public void SwitchToOn()
    {
        if (enabled) 
            return;

        enabled = true;
        if (_spriteChanger != null)
            _spriteChanger.enabled = true;
    }

    public void SwitchToOff()
    {
        if (!enabled)
            return;
        enabled = false;
        if (_spriteChanger != null)
            _spriteChanger.enabled = false;
    }

}
