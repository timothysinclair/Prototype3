using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaunchPad : MonoBehaviour
{
    public float aiLaunchForce = 100.0f;
    public Transform launchDirection;
    public float playerLaunchForce = 100.0f;

    private AudioSource audioSource;
    public AudioClip launchSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

            PlayLaunchSound();
        }
        else if (player)
        {
            launchVector *= playerLaunchForce;

            var rigidBody = other.gameObject.GetComponent<Rigidbody>();
            rigidBody.velocity = launchVector;

            PlayLaunchSound();
        }
    }

    public void PlayLaunchSound()
    {
        var newPitch = Random.Range(0.875f, 1.125f);
        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(launchSound, 0.3f);
    }
}
