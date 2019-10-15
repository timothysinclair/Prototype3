using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueSequence
{
    [SerializeField]
    public List<NPCDialogue> dialogues;
    public UnityEvent onDialogueEnd;

    public void OnEnd()
    {
        onDialogueEnd.Invoke();
    }
}

