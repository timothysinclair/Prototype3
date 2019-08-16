using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform playerTransform;
    public bool lookAtPlayer = false;
    public bool rotateAroundPlayer = false;
    public float cameraRotationSpeed = 5.0f;

    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {   
        cameraOffset = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPos = playerTransform.position + cameraOffset;

        this.transform.position = Vector3.Slerp(this.transform.position, newPos, smoothFactor);

        if (rotateAroundPlayer)
        {
            float xAxis = Input.GetAxis("Mouse X");
            Quaternion turnAngle = Quaternion.AngleAxis(xAxis * cameraRotationSpeed, Vector3.up);

            cameraOffset = turnAngle * cameraOffset;
        }

        if (lookAtPlayer || rotateAroundPlayer)
        {
            transform.LookAt(playerTransform);
        }
    }
}
