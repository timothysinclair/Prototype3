using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TeleportTriggerVolume : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;

    private Vector3 teleportPosition;

    private void Start()
    {
        teleportPosition = teleportDestination.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player playerRef = other.GetComponent<Player>();
            playerRef.TeleportPlayer(teleportPosition, false);
        }
    }
}
