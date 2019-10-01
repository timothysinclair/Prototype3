using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TotemEnemy : MonoBehaviour
{
    [Header("Turn settings")]

    [Tooltip("The number of different positions to visit every 360 degrees")]
    [SerializeField] private int turnPositions = 4;

    [Tooltip("Time spent in a position before turning")]
    [SerializeField] private float waitTime = 2.0f;

    [Tooltip("The amount of time it takes to turn to the next position")]
    [SerializeField] private float turnTime = 1.0f;
    [SerializeField] private bool clockwise = true;

    [SerializeField] private bool doTurn = true;

    [Header("Vision settings")]

    [SerializeField] private float visionRadius = 5.0f;
    [SerializeField] [Range(0.1f, 360.0f)] private float visionAngle = 90.0f;
    [SerializeField] private Vector3 headOffset;

    [Tooltip("The maximum difference in height between the player and the totem to still detect them")]
    [SerializeField] private float maxHeightDiff = 1.0f;

    [Tooltip("The amount of time that the player has to spend in the totem's vision before being teleported")]
    [SerializeField] private float detectionTime = 2.0f;

    [Tooltip("The position that the player will be teleported to after being detected")]
    [SerializeField] private Transform teleportDestination;

    [Tooltip("The layers that are counted as 'walls' - objects in these layers will block the totem's vision")]
    [SerializeField] private LayerMask wallLayers;

    private Player playerRef;
    private int currentTurn = 0;
    private bool isTurning = false;
    private float turnTimer = 0.0f;
    private float turnDirection = 1.0f;
    private bool playerDetected = false;

    // Stores teleport position on startup
    private Vector3 teleportPosition;

    private float detectionTimer = 0.0f;

    // Temporary (for testing)
    [Header("Temporary")]
    public MeshRenderer totemFace;
    public Material peacefulMat;
    public Material hostileMat;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Assert(playerRef, "Totem Enemy couldn't find player object to create a reference. Is there a player in the scene? Do they have the player tag?", this);

        turnTimer = waitTime;
        teleportPosition = teleportDestination.position;
    }

    private void Update()
    {
        CheckTurnState();

        bool wasPlayerDetected = playerDetected;
        SearchForPlayer();
        if (wasPlayerDetected && !playerDetected)
        {
            OnDetectionEnd();
        }

        if (playerDetected)
        {
            OnDetectionUpdate();
        }

        if (playerDetected) { totemFace.material = hostileMat; }
        else { totemFace.material = peacefulMat; }
    }

    private void CheckTurnState()
    {
        if (!doTurn) { return; }

        turnTimer -= Time.deltaTime;

        if (turnTimer <= 0.0f)
        {
            // Finished turning
            if (isTurning)
            {
                turnTimer = waitTime;

                var lookSeq = DOTween.Sequence();
                
                lookSeq.Append(totemFace.transform.DORotate(new Vector3(0.0f, 360.0f / turnPositions, 0.0f) * turnDirection, waitTime / 2.0f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
                lookSeq.Append(totemFace.transform.DORotate(new Vector3(0.0f, -360.0f / turnPositions, 0.0f) * turnDirection, waitTime / 2.0f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));

            }
            // Start turning
            else
            {
                Turn();
                turnTimer = turnTime;
            }

            isTurning = !isTurning;
        }
    }

    private void SearchForPlayer()
    {
        playerDetected = false;

        if (playerRef.IsCamoflauged()) { return; }

        Vector3 headPos = this.transform.position + headOffset;
        Vector3 toPlayer = playerRef.transform.position - headPos;
        float heightDiff = Mathf.Abs(toPlayer.y);
        toPlayer.y = 0.0f;

        // Player is out of vision radius
        if (toPlayer.magnitude > visionRadius) { return; }
        
        // Player is at too large a height difference to be detected
        if (heightDiff > maxHeightDiff) { return; }

        // Check if player is within vision cone
        Vector3 forwardVec = this.transform.forward;
        forwardVec.y = 0.0f;
        Vector3.Normalize(forwardVec);
        Vector3.Normalize(toPlayer);
        float angleDiff = Vector3.Angle(forwardVec, toPlayer); 

        // Player is detected
        if (angleDiff < (visionAngle / 2.0f))
        {
            toPlayer = playerRef.transform.position - headPos;
            // Raycast to check for walls
            if (!Physics.Raycast(headPos, toPlayer, toPlayer.magnitude, wallLayers))
            {
                if (!playerDetected)
                {
                    OnDetectionStart();
                }

                playerDetected = true;
            }
        }     
    }

    private void Turn()
    {
        if (!doTurn) { return; }

        if (clockwise) { turnDirection = 1.0f; }
        else { turnDirection = -1.0f; }

        currentTurn = (currentTurn + 1) % turnPositions;

        transform.DOLocalRotate(new Vector3(0.0f, 360.0f / turnPositions) * turnDirection, turnTime, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine)
            .SetLoops(1);
    }

    private void OnDetectionStart()
    {
    }

    private void OnDetectionEnd()
    {
        detectionTimer = 0.0f;
    }

    private void OnDetectionUpdate()
    {
        detectionTimer += Time.deltaTime;

        if (detectionTimer >= detectionTime)
        {
            TeleportPlayer();
            playerDetected = false;
            OnDetectionEnd();
        }
    }

    private void TeleportPlayer()
    {
        Debug.Log("Teleported Player");
        playerRef.TeleportPlayer(teleportPosition);
        // playerRef.GetComponentInParent<Transform>().position = teleportPosition;
        // playerRef.transform.position = teleportPosition;
    }

    private void OnDrawGizmos()
    {
        Vector3 headPos = this.transform.position + headOffset;

        UnityEditor.Handles.color = Color.red;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headPos, 0.65f);

        Vector3 lookDir = this.transform.forward;
        Vector3 leftBound = lookDir;
        Vector3 rightBound = lookDir;

        leftBound = Quaternion.Euler(0.0f, -visionAngle / 2.0f, 0.0f) * leftBound;
        rightBound = Quaternion.Euler(0.0f, visionAngle / 2.0f, 0.0f) * rightBound;

        leftBound = headPos + leftBound.normalized * visionRadius;
        rightBound = headPos + rightBound.normalized * visionRadius;

        Gizmos.DrawLine(headPos, leftBound);
        Gizmos.DrawLine(headPos, rightBound);
        
        UnityEditor.Handles.DrawWireArc(headPos, Vector3.up, leftBound - headPos, visionAngle, visionRadius);
    }
}
