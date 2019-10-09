using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCManager : MonoBehaviour
{
    private static NPCManager instance;

    public static NPCManager Instance { get { return instance; } }

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
        if (!dialogueBox.activeSelf) dialogueBox.SetActive(true);

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
        dialogueBox.SetActive(false);
    }
}
