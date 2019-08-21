using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Singleton
    private static PlayerUI instance;
    public static PlayerUI Instance { get { return instance; } }

    public GameObject textBoxPrefab;

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
}
