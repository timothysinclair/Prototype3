using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Singleton
    private static PlayerUI instance;
    public static PlayerUI Instance { get { return instance; } }

    public GameObject textBoxPrefab;
    public GameObject notebook;

    private bool notebookActive = false;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    public void CreateTempTextbox(string newText)
    {
        var textBox = Instantiate(textBoxPrefab, this.transform);
        textBox.GetComponent<TempTextbox>().SetText(newText);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (notebookActive)
            {
                notebook.SetActive(false);
            }
            else
            {
                notebook.SetActive(true);
            }

            var player = FindObjectOfType<PlayerController>();
            player.ToggleCursor();

            notebookActive = !notebookActive;
        }
    }
}
