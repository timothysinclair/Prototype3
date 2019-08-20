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
    public GameObject pauseOverlay;
    public GameObject pauseButtonObject;

    public Button actionButton;
    public Button pauseButton;

    public Sprite jumpSprite;
    public Sprite talkSprite;
    public Sprite eatSprite;
    public Sprite pauseSprite;
    public Sprite playSprite;

    public Camera hangiCamera;
    public Camera mainCamera;

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

        var hangi = FindObjectOfType<Hangi>();
        hangi.canActivate = true;
    }

    public void UpdateActionText(ActionState newState)
    {
        switch (newState)
        {
            case ActionState.eat:
                {
                    actionButton.GetComponent<Image>().sprite = eatSprite;
                    break;
                }

            case ActionState.talk:
                {
                    actionButton.GetComponent<Image>().sprite = talkSprite;
                    break;
                }

            case ActionState.jump:
                {
                    actionButton.GetComponent<Image>().sprite = jumpSprite;
                    break;
                }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pauseOverlay.SetActive(false);
            Time.timeScale = 1.0f;
            pauseButton.GetComponent<Image>().sprite = pauseSprite;

            player.DisableInputs(false);
        }
        else
        {
            pauseOverlay.SetActive(true);
            Time.timeScale = 0.0f;
            pauseButton.GetComponent<Image>().sprite = playSprite;

            pauseButtonObject.transform.SetAsLastSibling();

            player.DisableInputs(true);
        }

        isPaused = !isPaused;
    }

    public void StartHangi()
    {
        var hangi = FindObjectOfType<Hangi>();

        if (hangi) { hangi.ActivateFood(arrivedFriends); }

        mainCamera.enabled = false;
        hangiCamera.gameObject.SetActive(true);
        hangiCamera.enabled = true;

        player.DisableInputs(true);

    }
}
