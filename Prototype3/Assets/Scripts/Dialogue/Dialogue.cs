using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string header;

    [TextArea(2, 5)]
    public List<string> sentences;

    public void Load()
    {
        DialogueManager.Instance.Load(this);
    }
} 