using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalState
{
    LOCKED,
    UNLOCKED,
    GATEWAY
};

public class Portal : MonoBehaviour
{
    public PortalState portalState;
    public Transform location;
    public bool debug;

    public void Lock() { portalState = PortalState.LOCKED; }
    public void Unlock() { portalState = PortalState.UNLOCKED; }

    private void OnDrawGizmos()
    {
        //Target Location
        if (debug)
        {
            Gizmos.DrawIcon(location.position, "", true);

            //Portal
            Gizmos.color = new Color(0.3f, 0.0f, 0.4f, 0.30f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }


    public PortalState Next()
    {
        PortalState[] portalStates = (PortalState[])Enum.GetValues(portalState.GetType());
        int j = Array.IndexOf<PortalState>(portalStates, portalState) + 1;
        return (portalStates.Length == j) ? portalStates[0] : portalStates[j];
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if(portalState == PortalState.UNLOCKED && player != null)
        {
            player.StartTeleportPlayer(location.position);
        }
    }
}
