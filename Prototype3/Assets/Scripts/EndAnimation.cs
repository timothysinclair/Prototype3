using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndAnimation : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
