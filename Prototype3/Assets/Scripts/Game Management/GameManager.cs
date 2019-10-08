using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public Level[] gameLevels;

    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public Level GetCurrentLevel()
    {
        return gameLevels[currentLevelIndex];
    }
}
