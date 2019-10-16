using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCManager : MonoBehaviour
{
    private static NPCManager instance;

    public static NPCManager Instance { get { return instance; } }

    public GameObject dialogueBox, playerPanel;

    public Image playerAvatar, targetAvatar;
    public TMP_Text sentence;
    private Queue<NPCDialogue> dialogues;
    private bool talking = false;

    private NPC npc;

    private void Start()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }

        dialogues = new Queue<NPCDialogue>();
    }

    public void Load(NPC npc)
    {
        dialogues.Clear();

        this.npc = npc;
        foreach (NPCDialogue dialogue in npc.GetDialogues())
        {
            dialogues.Enqueue(dialogue);
        }

        this.npc.GetPlayer().GetInputs().SetInputsDisabled(true);

        playerAvatar.sprite = npc.GetDialogues()[0].playerEmote;
        targetAvatar.sprite = npc.GetDialogues()[0].targetEmote;
    }

    public void Next()
    {
        if (talking) return;
        if (playerPanel.activeSelf) playerPanel.SetActive(false);
        if (!dialogueBox.activeSelf) dialogueBox.SetActive(true);

        if (dialogues.Count <= 0)
        {
            Cancel();
            npc.SequenceEnd();
        }
        else
        {
            NPCDialogue dialogue = dialogues.Dequeue();
            sentence.text = dialogue.sentence;

            playerAvatar.sprite = dialogue.playerEmote;
            targetAvatar.sprite = dialogue.targetEmote;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogue.sentence));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        this.sentence.text = "";
        talking = true;
        foreach (char letter in sentence.ToCharArray())
        {
            this.sentence.text += letter;
            yield return null;
        }
        talking = false;
    }

    public void Cancel()
    {
        npc.GetPlayer().SetInputsDisabled(false);
        npc.GetPlayer().GetInputs().SetActionState(ActionState.jump);
        npc.Refresh();

        playerPanel.SetActive(true);
        dialogueBox.SetActive(false);
    }
}