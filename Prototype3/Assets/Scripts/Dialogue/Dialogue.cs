using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{ 
    public string header;

    [TextArea(2, 5)]
    public string[] sentenses;

    public void Load()
    {
        DialogueManager.Instance.Load(this);
    }
} 