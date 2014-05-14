using UnityEngine;
using System.Collections;

public class PlayOnClick : MonoBehaviour
{
    private RandomPlayAudio _randomPlayAudio;

    private void Start()
    {
        _randomPlayAudio = GetComponent<RandomPlayAudio>();    
    }


    private void OnClick()
    {
        if (_randomPlayAudio != null)
            _randomPlayAudio.Play();
        else if (audio != null)
            audio.Play();
    }
}
