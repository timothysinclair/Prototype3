using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



public class PlayerControllerRigidbody : MonoBehaviour
{
    [Header("Movement forces")]
    [SerializeField] private float moveForce = 10.0f;
    [SerializeField] private float jumpForce = 20.0f;

    [Header("Ground checking")]
    // Determines which layers count as the 'ground'
    [SerializeField] private LayerMask groundLayers;
    // The position to start the ground check from
    [SerializeField] private Transform groundChecker;

    // The radius of the spherecast that checks if the player is grounded
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Animator playerAnimator;

    [Header("Other settings")]
    // How many frames the player can still jump for if they fall off an edge
    [SerializeField] private int extraJumpFrames = 2;

    // The ratio of control the player has of their character in the air (1.0f is the same as on the ground)
    [SerializeField] [Range(0.01f, 1.0f)] private float airControl = 0.1f;

    // Private variables
    private Rigidbody rigidBody;
    private bool isGrounded = true;
    
    // Keeps track of whether or not the player was grounded in previous frames
    private List<bool> groundedFrames;

    // True if the player jumped after they went off an edge, but within the number of extra jump frames
    private bool lenientJump = false;

    // Stores the inputs of the player
    private Vector2 moveInputs;
    private bool jumpInput = false;

    // Stores the calculated move direction of the player
    private Vector2 finalMoveDirection;
    private bool finalJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        groundedFrames = new List<bool>(extraJumpFrames);

        for (int i = 0; i < extraJumpFrames; i++)
        {
            groundedFrames.Add(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
        playerAnimator.SetBool("Grounded", isGrounded);

        // Update list of grounded frames
        if (groundedFrames.Capacity > 0)
        {
            for ( int i = groundedFrames.Capacity - 1; i > 0; i--)
            {
                groundedFrames[i] = groundedFrames[i - 1];
            }
            groundedFrames[0] = isGrounded;
        }

        UpdateMoveInputs();

        // Check if the player jumped during lenient jump period
        lenientJump = false;

        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            if (groundedFrames[i]) { lenientJump = true; }
        }

    }

    // Called by player to move the player
    public void Move(Vector2 inputs, bool doJump)
    {
        moveInputs = inputs;

        if (doJump)
        {
            if (isGrounded || lenientJump)
            {
                jumpInput = true;
            }
            else
            {
                jumpInput = false;
            }
        }
    }

    private void UpdateMoveInputs()
    {
        // Find camera forward direction
        var camForward = cam.transform.forward;
        camForward.y = 0.0f;
        camForward.Normalize();

        // Find camera right direction
        var camRight = cam.transform.right;
        camRight.y = 0.0f;
        camRight.Normalize();

        // Update final move direction
        finalMoveDirection = Vector2.zero;
        finalMoveDirection = moveInputs.x * camRight + moveInputs.y * camForward;

        // Normalize move direction
        if (finalMoveDirection != Vector2.zero)
        {
            transform.forward = finalMoveDirection.normalized;
        }
    }

    private void FixedUpdate()
    {
        float airModifier = 1.0f;
        if (!isGrounded) { airModifier *= airControl; }

        rigidBody.AddForce(finalMoveDirection.normalized * moveForce * airModifier * Time.fixedDeltaTime);

        if (jumpInput)
        {
            Jump();
        }

        // Update player run animation based on speed
        if (rigidBody.velocity.magnitude > 1.0f) { playerAnimator.SetBool("Run", true); }
        else { playerAnimator.SetBool("Run", false); }
    }

    private void Jump()
    {
        Vector3 force = Vector3.up * jumpForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
        isGrounded = false;
        ResetGroundedFrames();
    }

    private void ResetGroundedFrames()
    {
        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            groundedFrames[i] = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }
}
