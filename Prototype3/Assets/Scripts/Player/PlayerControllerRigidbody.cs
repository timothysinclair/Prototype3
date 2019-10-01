using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerRigidbody : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private float moveForce = 10.0f;
    [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private float movingDrag = 0.5f;
    [SerializeField] private float stationaryDrag = 0.9f;
    [SerializeField] private float airDrag = 0.0f;

    [Tooltip("How much of normal move force should be applied while moving when camoflauged")]
    [SerializeField] [Range(0.01f, 1.0f)] private float camoflaugedSpeedModifier = 0.5f;

    // Affects how much drag affects the player
    [SerializeField] private float dragCoefficient = 10.0f;
    [SerializeField] private float maxSpeed = 20.0f;

    [Header("Ground checking")]
    // Determines which layers count as the 'ground'
    [SerializeField] private LayerMask groundLayers;
    // The position to start the ground check from
    [SerializeField] private Transform groundChecker;

    // The radius of the spherecast that checks if the player is grounded
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("References")]
    [SerializeField] private CinemachineFreeLook cam;
    [SerializeField] private Animator playerAnimator;

    [Header("Other settings")]
    // How many frames the player can still jump for if they fall off an edge
    [SerializeField] private int extraJumpFrames = 2;

    // The ratio of control the player has of their character in the air (1.0f is the same as on the ground)
    [SerializeField] [Range(0.01f, 1.0f)] private float airControl = 0.1f;

    // Private variables
    private Rigidbody rigidBody;
    public bool isGrounded = true;
    
    // Keeps track of whether or not the player was grounded in previous frames
    private List<bool> groundedFrames;

    // True if the player jumped after they went off an edge, but within the number of extra jump frames
    private bool lenientJump = false;

    // Current drag value, changes based on the state of the player
    private float currentDrag;

    // Stores the inputs of the player
    private Vector3 moveInputs;
    private bool jumpInput = false;

    private bool isCamoflauged = false;
    private float moveSpeedModifier = 1.0f;

    // Stores the calculated move direction of the player
    private Vector3 finalMoveDirection;

    [Header("FOR TESTING")]
    public Material camoflaugeMaterial;
    private Material normalMaterial;

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
    public void Move(Vector3 inputs, bool doJump, bool doCamoflauge)
    {
        moveInputs = inputs;

        if (doCamoflauge && !isCamoflauged && isGrounded)
        {
            isCamoflauged = true;
            OnStartCamoflauge();
        }
        else if (!doCamoflauge && isCamoflauged)
        {
            isCamoflauged = false;
            OnEndCamoflauge();
        }

        
        if (doJump && !isCamoflauged)
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
        else
        {
            jumpInput = false;
        }
    }

    private void UpdateMoveInputs()
    {
        // Find camera forward direction
        var camForward = this.transform.position - cam.transform.position;
        camForward.y = 0.0f;
        camForward.Normalize();

        //// Find camera right direction
        Vector3 camRight = new Vector3(camForward.z, 0.0f, -camForward.x);

        // Update final move direction
        finalMoveDirection = Vector3.zero;
        finalMoveDirection = moveInputs.x * camRight + moveInputs.z * camForward;

        // Normalize move direction
        if (finalMoveDirection != Vector3.zero)
        {
            transform.forward = finalMoveDirection.normalized;
        }
    }

    private void FixedUpdate()
    {
        float airModifier = 1.0f;
        if (!isGrounded) { airModifier *= airControl; }

        rigidBody.AddForce(finalMoveDirection.normalized * moveForce * moveSpeedModifier * airModifier * Time.fixedDeltaTime, ForceMode.Impulse);

        UpdateDrag();

        ApplyDrag();

        // Clamp to max speed
        CapSpeed();

        if (jumpInput)
        {
            Jump();
        }

        // Update player run animation based on speed
        if (rigidBody.velocity.magnitude > 3.0f) { playerAnimator.SetBool("Run", true); }
        else { playerAnimator.SetBool("Run", false); }
    }

    private void UpdateDrag()
    {
        if (!isGrounded)
        {
            currentDrag = airDrag;
        }
        else if (moveInputs == Vector3.zero)
        {
            currentDrag = stationaryDrag;
        }
        else
        {
            currentDrag = movingDrag;
        }
    }

    private void ApplyDrag()
    {
        var currentVelocity = rigidBody.velocity;
        currentVelocity.y = 0.0f;

        // Apply 'drag'
        rigidBody.AddForce(-rigidBody.velocity * currentDrag * dragCoefficient * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void CapSpeed()
    {
        var currentVelocity = rigidBody.velocity;

        if (currentVelocity.magnitude > (maxSpeed * moveSpeedModifier))
        {
            Vector3 newVelocity;
            newVelocity.y = currentVelocity.y;

            currentVelocity.y = 0.0f;
            currentVelocity = currentVelocity.normalized * maxSpeed * moveSpeedModifier;
            newVelocity.x = currentVelocity.x;
            newVelocity.z = currentVelocity.z;

            rigidBody.velocity = newVelocity;
        }
    }

    private void Jump()
    {
        Vector3 force = Vector3.up * jumpForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
        isGrounded = false;
        ResetGroundedFrames();
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        Vector3 positionChange = newPosition - this.transform.position;
        Vector3 relativeCamPos = cam.transform.position - this.transform.position;

        this.transform.position = newPosition;

        cam.OnTargetObjectWarped(this.transform, positionChange);
    }

    private void ResetGroundedFrames()
    {
        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            groundedFrames[i] = false;
        }
    }

    private void OnStartCamoflauge()
    {
        var renderer = playerAnimator.GetComponentInChildren<SkinnedMeshRenderer>();
        var mats = renderer.materials;
        normalMaterial = mats[2];
        mats[2] = camoflaugeMaterial;
        renderer.materials = mats;
        moveSpeedModifier = camoflaugedSpeedModifier;
        
    }

    private void OnEndCamoflauge()
    {
        var renderer = playerAnimator.GetComponentInChildren<SkinnedMeshRenderer>();
        var mats = renderer.materials;
        mats[2] = normalMaterial;
        renderer.materials = mats;
        moveSpeedModifier = 1.0f;
    }

    public bool IsCamoflauged() { return isCamoflauged; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }
}
