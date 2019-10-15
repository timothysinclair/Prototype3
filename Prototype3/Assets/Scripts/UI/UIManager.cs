using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] private Transform panel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(paused)
            {
                paused = false;
                panel.gameObject.SetActive(false);
                Time.timeScale = 1;


                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                paused = true;
                panel.gameObject.SetActive(true);
                Time.timeScale = 0;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
