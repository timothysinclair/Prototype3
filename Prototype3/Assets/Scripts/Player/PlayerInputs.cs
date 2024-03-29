﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Determines the action that is used when the player presses the action button
public enum ActionState
{
    jump,
    talk
}

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private PlayerControllerRigidbody playerController;

    private Vector3 moveInputs = Vector3.zero;
    private bool jumpInput = false;
    public ActionState actionState = ActionState.jump;
    private bool inputsDisabled = false;
    private bool cursorLock = false;
    private Player playerRef;

    public Image actionImage;
    public Sprite jumpImage;
    public Sprite talkImage;

    private void Awake()
    {
        playerRef = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputsDisabled) {
            moveInputs.x = Input.GetAxisRaw("Horizontal");
            moveInputs.z = Input.GetAxisRaw("Vertical");

            bool camouflageInput = Input.GetButton("Camouflage");
            // bool camouflageInput = (playerRef.HeldCrystalType() == CrystalType.Purple);

            playerController.Move(moveInputs, jumpInput, camouflageInput);
        }

        switch (actionState)
        {
            default:
                break;
            case ActionState.jump:
                {
                    jumpInput = Input.GetButtonDown("Jump");
                    actionImage.sprite = jumpImage;
                    break;
                }

            case ActionState.talk:
                {
                    if (Input.GetButtonDown("Jump")) NPCManager.Instance.Next();
                    actionImage.sprite = talkImage;
                    break;
                }
        }
    }

    public void SetActionState(ActionState actionState)
    {
        this.actionState = actionState;
    }

    public void ToggleCursorLock()
    {
        if (cursorLock)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        cursorLock = !cursorLock;
    }

    public void SetInputsDisabled(bool disabled)
    {
        jumpInput = false;
        moveInputs = Vector3.zero;
        inputsDisabled = disabled;
    }

    public bool GetInputsDisabled()
    {
        return inputsDisabled;
    }
}
