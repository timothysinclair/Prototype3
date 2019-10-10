using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPCName
{
    Ariki = 0,
    Manu = 1,
    Rangi = 2
}

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Material teleportMaterial;

    // Teleporting timing & materials
    [SerializeField] private MeshRenderer meshRenderer;
    private Material normalMaterial;
    private Vector3 teleportDestination;
    private bool isTeleporting = false;
    private float teleportDir = 1.0f;
    private float teleportTimer = 0.0f;
    [SerializeField] private float teleportTime = 1.0f;

    private void Start()
    {
        agent.isStopped = true;
        normalMaterial = meshRenderer.material;
        teleportMaterial = new Material(teleportMaterial);
    }

    private void Update()
    {
        UpdateMaterials();
    }

    public void SendTo(Transform destination)
    {
        agent.SetDestination(destination.position);
        agent.isStopped = false;
    }

    public void Pause()
    {
        agent.isStopped = true;
    }

    public void Resume()
    {
        agent.isStopped = false;
    }
    
    public void StartTeleport(Transform destination)
    {
        teleportDestination = destination.position;
        teleportTimer = teleportTime;
        teleportDir = 1.0f;
        isTeleporting = true;
    }

    private void Teleport()
    {
        agent.Warp(teleportDestination);
    }

    private void UpdateMaterials()
    {
        if (isTeleporting)
        {
            teleportTimer -= Time.deltaTime * teleportDir;

            if (teleportTimer <= 0.0f)
            {
                Teleport();
                teleportDir = -1.0f;
            }
            else if (teleportTimer >= teleportTime)
            {
                isTeleporting = false;
            }


            meshRenderer.material = teleportMaterial;
            teleportMaterial.SetFloat("_TimeScale", 1.0f - (teleportTimer / teleportTime));
        }
        else
        {
            meshRenderer.material = normalMaterial;
        }
    }
}
