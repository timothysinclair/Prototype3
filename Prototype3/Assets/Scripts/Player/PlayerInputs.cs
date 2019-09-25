using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Determines the action that is used when the player presses the action button
public enum ActionState
{
    jump,
    talk
}

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private PlayerControllerRigidbody playerController;

    public Vector2 moveInputs = Vector3.zero;
    private bool jumpInput = false;
    private ActionState actionState = ActionState.jump;
    private bool inputsDisabled = false;
    private bool cursorLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputsDisabled) { return; }

        moveInputs.x = Input.GetAxisRaw("Horizontal");
        moveInputs.y = Input.GetAxisRaw("Vertical");
        jumpInput = false;

        switch (actionState)
        {
            default:
            case ActionState.jump:
            {
                jumpInput = Input.GetButtonDown("Jump");
                break;
            }

            case ActionState.talk:
            {

                break;
            }
        }

        playerController.Move(moveInputs, jumpInput);
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
        inputsDisabled = disabled;
    }

    public bool GetInputsDisabled()
    {
        return inputsDisabled;
    }
}
