using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public static DialogueManager Instance { get { return instance; } }

    public GameObject dialogueBox;
    public TMP_Text author, sentence;
    private Queue<string> sentences;

    private void Start()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }

        sentences = new Queue<string>();
    }

    public void Load(Dialogue dialogue)
    {
        Debug.Log("Start Conversation");
        dialogueBox.SetActive(true);
        author.text = dialogue.header;

        foreach (string sentence in dialogue.sentenses)
        {
            sentences.Enqueue(sentence);
        }
        Next();
    }

    public void Next()
    {
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
        sentences.Clear();
        Debug.Log("Finished Conversation.");
        dialogueBox.SetActive(false);

    }
}
