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
    public GameObject pauseOverlay;
    public GameObject pauseButton;
    public TextMeshProUGUI pauseButtonText;

    private int friendCount = 0;
    private int arrivedFriends = 0;
    private bool isPaused = false;
    private PlayerController player;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    private void Start()
    {
        friendCount = FindObjectsOfType(typeof(Friend)).Length;
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { TogglePause(); }
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

    public void TogglePause()
    {
        if (isPaused)
        {
            pauseOverlay.SetActive(false);
            Time.timeScale = 1.0f;
            pauseButtonText.text = "Pause";

            player.DisableInputs(false);
        }
        else
        {
            pauseOverlay.SetActive(true);
            Time.timeScale = 0.0f;
            pauseButtonText.text = "Play";
            pauseButton.transform.SetAsLastSibling();

            player.DisableInputs(true);
        }

        isPaused = !isPaused;
    }
}
