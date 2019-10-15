using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool paused = false;
    public string mainMenuScene;

    [SerializeField] private Transform pausePanel;
    [SerializeField] private Transform playerPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(paused)
            {
                paused = false;
                pausePanel.gameObject.SetActive(false);
                playerPanel.gameObject.SetActive(true);
                Time.timeScale = 1;


                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                paused = true;
                pausePanel.gameObject.SetActive(true);
                playerPanel.gameObject.SetActive(false);
                Time.timeScale = 0;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void OnMenuPress()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
