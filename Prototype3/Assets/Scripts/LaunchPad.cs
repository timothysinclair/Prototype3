using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaunchPad : MonoBehaviour
{
    public float aiLaunchForce = 100.0f;
    public Transform launchDirection;
    public float playerLaunchForce = 100.0f;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 launchVector = Vector3.Normalize(launchDirection.position - this.transform.position);

        var friend = other.GetComponent<Friend>();
        var player = other.GetComponent<PlayerController>();

        if (friend)
        {
            launchVector *= aiLaunchForce;

            friend.Launch();
            var rigidBody = other.gameObject.GetComponent<Rigidbody>();
            rigidBody.velocity = launchVector;
        }
        else if (player)
        {
            launchVector *= playerLaunchForce;

            var rigidBody = other.gameObject.GetComponent<Rigidbody>();
            rigidBody.velocity = launchVector;
        }
    }
}
