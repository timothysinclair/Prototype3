using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General Settings")]
    public Transform cameraTransform;
    public float gravity = 15.0f;
    public float rotationSpeed = 240.0f;

    [Range(0.01f, 25.0f)]
    public float speed = 5.0f;

    [Header("Camera Settings")]
    [Range(0.01f, 1.0f)]
    public float smoothing = 0.5f;
    public float cameraRotationSpeed = 5.0f;

    public bool lookAtPlayer = false;
    public bool rotateAroundPlayer = false;

    private Vector3 cameraOffset;
    private Vector3 movementDirection = Vector3.zero;
    private CharacterController characterController;

    private Vector3 force = Vector3.zero;

    private void Start()
    {
        cameraOffset = (cameraTransform.position - transform.position) + new Vector3(2.0f, 0.0f, 0.0f);
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get Input for axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the forward vector
        Vector3 forwardDirection = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = vertical * forwardDirection + horizontal * cameraTransform.right;

        if (movement.magnitude > 1f) movement.Normalize();
        movement = transform.InverseTransformDirection(movement);

        float turnAmount = Mathf.Atan2(movement.x, movement.z);
        transform.Rotate(0, turnAmount * rotationSpeed * Time.deltaTime, 0);

        if (characterController.isGrounded)
        { 
            movementDirection = transform.forward * movement.magnitude;
            movementDirection *= speed;
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);

        UpdateForce(Time.deltaTime);
    }
     
    private void LateUpdate()
    {
        if (rotateAroundPlayer)
        {
            float xAxis = Input.GetAxis("Mouse X");
            Quaternion turnAngle = Quaternion.AngleAxis(xAxis* cameraRotationSpeed, Vector3.up);

            cameraOffset = turnAngle * cameraOffset;
        }

        Vector3 position = transform.position + cameraOffset;
        cameraTransform.position = Vector3.Slerp(cameraTransform.position, position, smoothing);

        if (lookAtPlayer || rotateAroundPlayer) cameraTransform.LookAt(transform);
    } 

    public void AddForce(Vector3 newForce)
    {
        force += newForce;
    }

    private void UpdateForce(float deltaTime)
    {
        if (force.magnitude > 0.2f)
        {
            characterController.Move(force * deltaTime);
        }
        force = Vector3.Lerp(force, Vector3.zero, 2 * deltaTime);
    }
}
