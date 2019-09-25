using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public new string name;

    [SerializeField]
    private List<Dialogue> dialogues;

    private void Start()
    {
        foreach(Dialogue dialogue in dialogues)
        {
            dialogue.header = name;
        }
    }

    public void Conversate(int id)
    {
        dialogues[id].Load();
    }

    public List<Dialogue> GetDialogues()
    {
        return dialogues;
    }
}
