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
    private Queue<string> sentences;

    private NPC npc;

    private void Start()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }

        sentences = new Queue<string>();
    }

    public void Load(NPC npc)
    {
        Debug.Log("Started Conversation.");
        sentences.Clear();

        this.npc = npc;
        this.avatar = npc.avatar;
        author.text = npc.name;

        foreach (string sentence in npc.GetSentences())
        {
            sentences.Enqueue(sentence);
        }
    }

    public void Next()
    {
        if(!dialogueBox.activeSelf) dialogueBox.SetActive(true);

        if (sentences.Count <= 0)
        {
            Cancel();
        }
        else
        {
            sentence.text = sentences.Dequeue();
            Debug.Log(sentence.text);
        }
    }

    public void Cancel()
    {
        npc.Refresh();
        sentences.Clear();
        Debug.Log("Finished Conversation.");
        dialogueBox.SetActive(false);
    }
}
