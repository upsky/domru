﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ButtonLoadCurrentLevel : MonoBehaviour
{
    //[SerializeField]
    //private Consts.SceneNames _sceneName;

    private void Start()
    {
    }

    private void OnClick()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
}
