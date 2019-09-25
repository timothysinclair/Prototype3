using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private bool lookAtPlayer = false;
    [SerializeField] private bool rotateAroundPlayer = false;
    [SerializeField] private float cameraRotationSpeed = 5.0f;
    [SerializeField] [Range(0.01f, 1.0f)] private float smoothFactor = 0.5f;
    [SerializeField] private Player player;

    private Vector3 cameraOffset;
    

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = this.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetInputsDisabled()) { return; }

        Vector3 newPos = player.transform.position + cameraOffset;

        this.transform.position = Vector3.Slerp(this.transform.position, newPos, smoothFactor);

        if (rotateAroundPlayer)
        {
            float xAxis = Input.GetAxis("Mouse X");
            float yAxis = -Input.GetAxis("Mouse Y");

            Quaternion turnAngle = Quaternion.AngleAxis(xAxis * cameraRotationSpeed, Vector3.up) * Quaternion.AngleAxis(yAxis * cameraRotationSpeed, this.transform.right);
            cameraOffset = turnAngle * cameraOffset;
        }

        if (lookAtPlayer)
        {
            transform.LookAt(player.transform);
        }
    }
}
