using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private UnityEvent onPuzzleCompleted;

    [SerializeField] private Transform playerFallRespawn;

    private Vector3 playerFallRespawnPos;
    private Player playerRef;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerFallRespawnPos = playerFallRespawn.position;
    }

    public void RespawnPlayer()
    {
        playerRef.StartTeleportPlayer(playerFallRespawnPos);
    }
}
