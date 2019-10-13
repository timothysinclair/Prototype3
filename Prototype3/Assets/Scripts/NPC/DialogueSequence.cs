﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueSequence
{
    public List<string> sentences;
    public UnityEvent onDialogueEnd;

    public void OnEnd()
    {
        onDialogueEnd.Invoke();
    }
}