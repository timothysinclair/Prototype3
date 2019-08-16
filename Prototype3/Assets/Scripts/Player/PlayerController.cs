using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isGrounded = true;
    private Camera cam;

    private List<bool> groundedFrames;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        groundedFrames = new List<bool>(extraJumpFrames);

        for (int i = 0; i < extraJumpFrames; i++)
        {
            groundedFrames.Add(false);
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayers, QueryTriggerInteraction.Ignore);

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
        }

        if (inputs != Vector3.zero)
        {
            transform.forward = inputs.normalized;
        }

        // Check if player jumped within lenient jump frames
        bool lenientJump = false;

        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            if (groundedFrames[i]) { lenientJump = true; }
        }

        if ((Input.GetButtonDown("Jump") && isGrounded) || (Input.GetButtonDown("Jump") && lenientJump))
        {
            Jump();
        }

    }

    private void FixedUpdate()
    {
        float airModifier = 1.0f;
        if (!isGrounded) { airModifier *= airControl; }

        rigidBody.AddForce(inputs.normalized * moveForce * airModifier * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundDistance);
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        ResetGroundedFrames();
    }

    private void ResetGroundedFrames()
    {
        for (int i = 0; i < groundedFrames.Capacity; i++)
        {
            groundedFrames[i] = false;
        }
    }
}
