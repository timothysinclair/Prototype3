using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour
{
    public new string name;
    public int conversationDistance = 3;

    private int progressionCounter = 0;

    private bool conversating = false;
    private bool checkRanged = true;

    public List<DialogueSequence> dialogueSequences;

    private Player player;

    private void Start()
    {
        Gizmos.color = Color.green;
        player = FindObjectOfType<Player>();
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            if (InRange())
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (!conversating && InRange())
            {
                if (checkRanged)
                {
                    if (NPCManager.Instance != null)
                    {
                        conversating = true;
                        checkRanged = false;
                        NPCManager.Instance.Load(this);
                        player.GetInputs().SetActionState(ActionState.talk);
                    }
                }
            }
            else
            {
                if (!checkRanged && !InRange())
                {
                    if (NPCManager.Instance != null)
                    {
                        conversating = false;
                        checkRanged = true;
                        NPCManager.Instance.Cancel();
                        player.GetInputs().SetActionState(ActionState.jump);
                    }
                }
            }
        }
    }

    private bool InRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < conversationDistance;
    }

    public void Refresh()
    {
        conversating = false;
        checkRanged = true;
    }

    public List<NPCDialogue> GetDialogues()
    {
        return dialogueSequences[progressionCounter].dialogues;
    }

    public void SequenceEnd()
    {
        dialogueSequences[progressionCounter].OnEnd();
    }

    public void ProgressDialogue()
    {
        progressionCounter++;

        Debug.Assert(progressionCounter <= dialogueSequences.Count, "NPC dialogue progression was incremented higher than allowed. Check where ProgressDialogue is being called and how many dialogue sequences you have");
    }

    public void SetProgression(int newProgression)
    {
        progressionCounter = newProgression;
    }
}