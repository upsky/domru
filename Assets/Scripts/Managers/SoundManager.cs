using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private AudioClip _titleClip;

    private RandomPlayAudio _randomPlayAudio;


    private void Start()
    {
        _randomPlayAudio = GetComponent<RandomPlayAudio>();
    }

    private void Update()
    {
        if (audio.isPlaying)
            return;

        if (Application.loadedLevelName.Contains("Room") || Application.loadedLevelName == Consts.SceneNames.Level1.ToString())
            _randomPlayAudio.Play();
        else
        {
            audio.clip = _titleClip;
            audio.Play();
        }
    }

}

