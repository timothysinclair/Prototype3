using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    jump,
    talk,
    eat
}

public class PlayerController : MonoBehaviour
{
    public float moveForce = 10.0f;
    public float jumpForce = 20.0f;
    public float groundDistance = 0.2f;
    public LayerMask groundLayers;
    public Transform groundChecker;

    public float movingDrag = 0.5f;
    public float normalDrag = 0.9f;
    public float airDrag = 0.0f;

    public int extraJumpFrames = 2;

    [Range(0.01f, 1.0f)]
    public float airControl = 0.1f;

    private Rigidbody rigidBody;
    public Vector3 inputs = Vector3.zero;
    public bool isGrounded = true;
    private Camera cam;

    private Vector3 lastSafePosition;

    private List<bool> groundedFrames;
    private bool lenientJump = false;
    private bool externalAction = false;

    private bool inputsDisabled = false;

    private bool inTalkingDistance = false;
    private Friend talkingFriend;

    public AudioClip jumpSound;
    private AudioSource audioSource;

    private bool cursorActive = false;
    private ActionState playerActionState = ActionState.jump;
    private bool doJump = false;

    public Animator playerAnimator;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;

        ToggleCursor();

        groundedFrames = new List<bool>(extraJumpFrames);

        for (int i = 0; i < extraJumpFrames; i++)
        {
            groundedFrames.Add(false);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCursor();
        }

        if (inputsDisabled) { return; }

        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayers, QueryTriggerInteraction.Ignore);

        if (isGrounded) { lastSafePosition = this.transform.position; }

        if (groundedFrames.Capacity > 0)
        {
            for (int i = groundedFrames.Capacity - 1; i > 0; i--)
            {
                groundedFrames[i] = groundedFrames[i - 1];
            }
            groundedFrames[0] = isGrounded;
        }

        var camForward = cam.transform.forward;
        camForward.y = 0.0f;
        camForward.Normalize();

        var camRight = cam.transform.right;
        camRight.y = 0.0f;
        camRight.Normalize();

        inputs = Vector3.zero;
        inputs = Input.GetAxis("Horizontal") * camRight + Input.GetAxis("Vertical") * camForward;

        if (!isGrounded)
        {
            rigidBody.drag = airDrag;
        }
        else if (inputs != Vector3.zero)
        {
            rigidBody.drag = movingDrag;
        }
        else
        {
            rigidBody.drag = normalDrag;
            rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (inputs != Vector3.zero)
        {
            transform.forward = inputs.normalized;
        }

        // Check if player jumped within lenient jump frames
        lenientJump = false;

        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            if (groundedFrames[i]) { lenientJump = true; }
        }

        if (Input.GetButtonDown("Jump") || externalAction)
        {
            TryAction();
        }

        externalAction = false;
    }

    public void Action()
    {
        externalAction = true;
    }

    private void TryAction()
    {
        switch (playerActionState)
        {
            case ActionState.eat:
                {
                    GameManager.Instance.StartHangi();
                    break;
                }

            case ActionState.talk:
                {
                    talkingFriend.Talk();
                    break;
                }

            case ActionState.jump:
                {
                    if (isGrounded || lenientJump)
                    {
                        doJump = true;
                        
                    }
                    break;
                }
        }
    }

    public void DisableInputs(bool disabled)
    {
        inputsDisabled = disabled;
    }

    public bool AreInputsDisabled()
    {
        return inputsDisabled;
    }

    private void FixedUpdate()
    {
        float airModifier = 1.0f;
        if (!isGrounded) { airModifier *= airControl; }

        rigidBody.AddForce(inputs.normalized * moveForce * airModifier * Time.fixedDeltaTime);

        if (doJump)
        {
            Jump();
        }

        if (rigidBody.velocity.magnitude > 1.0f) { playerAnimator.SetBool("Run", true); }
        else { playerAnimator.SetBool("Run", false); }

        doJump = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundDistance);
    }

    private void Jump()
    {
        Vector3 force = Vector3.up * jumpForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
        PlayJumpSound();
        //rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
        ResetGroundedFrames();
    }

    private void ResetGroundedFrames()
    {
        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            groundedFrames[i] = false;
        }
    }

    public void ReturnToSafePosition()
    {
        this.transform.position = lastSafePosition;
        rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SetActionState(ActionState newState)
    {
        playerActionState = newState;
        GameManager.Instance.UpdateActionText(newState);
    }

    public void SetFriend(Friend newFriend)
    {
        talkingFriend = newFriend;
    }

    public void ToggleCursor()
    {
        if (cursorActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cursorActive = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cursorActive = true;
        }
    }

    private void PlayJumpSound()
    {
        var newPitch = Random.Range(0.875f, 1.125f);
        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(jumpSound, 0.5f);
    }

    
}
