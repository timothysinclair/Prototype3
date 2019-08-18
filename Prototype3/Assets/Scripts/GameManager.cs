using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }


    // Testing UI
    public TextMeshProUGUI numFriends;
    public TextMeshProUGUI actionText;

    private int friendCount = 0;
    private int arrivedFriends = 0;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    private void Start()
    {
        friendCount = FindObjectsOfType(typeof(Friend)).Length;
        
    }

    private void FixedUpdate()
    {
        numFriends.text = "" + arrivedFriends + "/" + friendCount;
    }

    public void FriendArrived()
    {
        arrivedFriends++;
    }

    public void InTalkRange(bool inRange)
    {
        if (inRange)
        {
            actionText.text = "Talk";
        }
        else
        {
            actionText.text = "Jump";
        }
    }
}
