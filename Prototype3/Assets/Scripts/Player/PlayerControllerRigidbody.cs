using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRigidbody : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private float moveForce = 10.0f;
    [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private float movingDrag = 0.5f;
    [SerializeField] private float stationaryDrag = 0.9f;
    [SerializeField] private float airDrag = 0.0f;

    [Tooltip("How much of normal move force should be applied while moving when camouflaged")]
    [SerializeField] [Range(0.01f, 1.0f)] private float camouflagedSpeedModifier = 0.5f;

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
    [SerializeField] private Material respawnMaterial;
    [SerializeField] private Material teleportMaterial;

    [Header("Shader properties")]
    [SerializeField] private float teleportTime = 1.0f;

    // Totems 'respawn' the player
    private bool isRespawning = false;

    // Portals 'teleport' the player
    private bool isTeleporting = false;

    private float respawnTimer = 0.0f;
    private float teleportTimer = 0.0f;
    private float respawnDir = 1.0f;
    private float teleportDir = 1.0f;

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

    private Vector3 teleportPos;

    public bool canCamouflage = false;
    private bool isCamouflaged = false;
    private float moveSpeedModifier = 1.0f;

    private CrystalHolder playerHolder;

    // Stores the calculated move direction of the player
    private Vector3 finalMoveDirection;

    // Audio
    private AudioSource audioSource;
    private AudioClip jumpSound;
    private AudioClip teleportDepartSound;
    private AudioClip teleportArriveSound;
    private AudioClip respawnSound;

    [Header("FOR TESTING")]
    public Material camouflageMaterial;
    public Material normalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerHolder = GetComponent<CrystalHolder>();

        jumpSound = AudioManager.Instance.GetAudioClip("Jump");
        teleportDepartSound = AudioManager.Instance.GetAudioClip("TeleportDepart");
        teleportArriveSound = AudioManager.Instance.GetAudioClip("TeleportArrive");
        respawnSound = AudioManager.Instance.GetAudioClip("TotemTeleport");

        respawnMaterial = new Material(respawnMaterial);
        teleportMaterial = new Material(teleportMaterial);

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
        Vector3 moveSpeed = rigidBody.velocity;
        Vector3 hSpeed = new Vector3(moveSpeed.x, 0.0f, moveSpeed.z);
        Vector3 vSpeed = new Vector3(0.0f, moveSpeed.y, 0.0f);
        playerAnimator.SetFloat("VSpeed", vSpeed.magnitude);
        playerAnimator.SetBool("Camouflaged", isCamouflaged);
        playerAnimator.SetBool("Holding", (playerHolder.heldType != CrystalType.None));

        UpdateMaterials();

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
    public void Move(Vector3 inputs, bool doJump, bool doCamouflage)
    {
        moveInputs = inputs;

        if (canCamouflage && doCamouflage && !isCamouflaged && isGrounded)
        {
            isCamouflaged = true;
            OnStartCamouflage();
        }
        else if (!doCamouflage && isCamouflaged)
        {
            isCamouflaged = false;
            OnEndCamouflage();
        }
        
        if (doJump && !isCamouflaged)
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

        // Find camera right direction
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

        moveInputs = Vector3.zero;
        jumpInput = false;

        // Update player run animation based on speed
        if (rigidBody.velocity.magnitude > 2.0f) { playerAnimator.SetBool("Run", true); }
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
        audioSource.PlayOneShot(jumpSound);
        playerAnimator.SetTrigger("Jump");

        Vector3 force = Vector3.up * jumpForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
        isGrounded = false;
        ResetGroundedFrames();
    }

    public void SetRespawnTimer(float newTimer, Vector3 respawnPos)
    {
        if (isTeleporting) { return; }

        isRespawning = true;
        
        if (newTimer > respawnTimer)
        {
            respawnTimer = newTimer;
            teleportPos = respawnPos;
            respawnDir = 1.0f;
        }

        respawnTimer = Mathf.Max(newTimer, respawnTimer);
    }

    public void StartTeleportPlayer(Vector3 newPosition)
    {
        audioSource.PlayOneShot(teleportDepartSound);

        teleportPos = newPosition;
        teleportTimer = teleportTime;
        teleportDir = 1.0f;
        isTeleporting = true;
        GetComponent<Player>().SetInputsDisabled(true);
    }

    public void TeleportPlayer(bool respawned)
    {
        if (respawned)
        {
            audioSource.PlayOneShot(respawnSound);
        }
        else
        {
            audioSource.PlayOneShot(teleportArriveSound);
        }

        Vector3 positionChange = teleportPos - this.transform.position;
        Vector3 relativeCamPos = cam.transform.position - this.transform.position;

        this.transform.position = teleportPos;
        rigidBody.velocity = Vector3.zero;

        cam.OnTargetObjectWarped(this.transform, positionChange);
    }

    public void TeleportPlayer(Vector3 pos, bool respawned)
    {
        teleportPos = pos;
        TeleportPlayer(respawned);
    }

    private void ResetGroundedFrames()
    {
        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            groundedFrames[i] = false;
        }
    }

    public void UnlockCamouflage()
    {
        canCamouflage = true;
    }

    private void OnStartCamouflage()
    {
        // SetMaterial(camouflageMaterial);
        playerAnimator.SetTrigger("StartCamo");
        moveSpeedModifier = camouflagedSpeedModifier;
    }

    private void OnEndCamouflage()
    {
        // SetMaterial(normalMaterial);
        playerAnimator.SetTrigger("StopCamo");
        moveSpeedModifier = 1.0f;
    }

    public bool IsCamouflaged() { return isCamouflaged; }

    private void SetMaterial(Material newMaterial)
    {
        var renderer = playerAnimator.GetComponentInChildren<SkinnedMeshRenderer>();
        var mats = renderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = newMaterial;
        }

        renderer.materials = mats;
    }


    private void UpdateMaterials()
    {
        if (isTeleporting)
        {
            teleportTimer -= Time.deltaTime * teleportDir;

            if (teleportTimer <= 0.0f)
            {
                TeleportPlayer(false);
                teleportDir = -1.0f;
            }
            else if (teleportTimer >= teleportTime)
            {
                GetComponent<Player>().SetInputsDisabled(false);
                isTeleporting = false;
            }
            
            SetMaterial(teleportMaterial);
            teleportMaterial.SetFloat("_TimeScale", 1.0f - (teleportTimer / teleportTime));
        }
        else if (isRespawning)
        {
            // If dissolving back in, update timer
            if (respawnDir == -1.0f)
            {
                respawnTimer -= Time.deltaTime;
            }

            // Dissolving out
            if (respawnTimer >= 1.0f)
            {
                respawnDir = -1.0f;
            }
            // Finished dissolving in
            else if (respawnTimer <= 0.0f)
            {
                isRespawning = false;
            }

            SetMaterial(respawnMaterial);

            respawnMaterial.SetFloat("_TimeScale", respawnTimer);
        }
        else
        {
            SetMaterial(normalMaterial);
        }

        
        if (respawnDir == 1.0f)
        {
            isRespawning = false;
            respawnTimer = 0.0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }
}
