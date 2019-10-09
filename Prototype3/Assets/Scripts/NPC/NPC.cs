using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public new string name;
    public Image avatar;
    public int conversationDistance = 3;

    private bool conversating = false;
    private bool checkRanged = true;

    [SerializeField]
    private List<string> sentences;

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
        if(player != null)
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

    public List<string> GetSentences()
    {
        return sentences;
    }
}
