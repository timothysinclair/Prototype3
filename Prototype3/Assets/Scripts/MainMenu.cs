﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playButtonScene;

    public void OnPressPlay()
    {
        SceneManager.LoadSceneAsync(playButtonScene);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}