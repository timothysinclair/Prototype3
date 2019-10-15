using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public static DialogueManager Instance { get { return instance; } }

    public GameObject dialogueBox;
    public Image avatar;
    public TMP_Text author, sentence;
    private Queue<NPCDialogue> dialogues;

    private NPC npc;

    private void Start()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }

        dialogues = new Queue<NPCDialogue>();
    }

    public void Load(NPC npc)
    {
        Debug.Log("Started Conversation.");
        dialogues.Clear();

        this.npc = npc;
        author.text = npc.name;

        foreach (NPCDialogue dialogue in npc.GetDialogues())
        {
            dialogues.Enqueue(dialogue);
        }
    }

    public void Next()
    {
        if(!dialogueBox.activeSelf) dialogueBox.SetActive(true);

        if (dialogues.Count <= 0)
        {
            Cancel();
        }
        else
        {
            NPCDialogue npcDialogue = dialogues.Dequeue();
            sentence.text = npcDialogue.sentence;
            avatar.sprite = npcDialogue.targetEmote;
            Debug.Log(sentence.text);
        }
    }

    public void Cancel()
    {
        npc.Refresh();
        dialogues.Clear();
        Debug.Log("Finished Conversation.");
        dialogueBox.SetActive(false);
    }
}
