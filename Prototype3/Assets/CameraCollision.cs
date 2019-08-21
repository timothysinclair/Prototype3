using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    int iteration = 0;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(iteration++);
    }
}
