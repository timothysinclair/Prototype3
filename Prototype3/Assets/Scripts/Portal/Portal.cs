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
    public Portal targetPortal;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f, 0.0f, 0.5f, 0.70f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }


    public PortalState Next()
    {
        PortalState[] portalStates = (PortalState[])Enum.GetValues(portalState.GetType());
        int j = Array.IndexOf<PortalState>(portalStates, portalState) + 1;
        return (portalStates.Length == j) ? portalStates[0] : portalStates[j];
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if(targetPortal != null)
        {
            //if(player != null) player.Teleport(targetPortal.transform);
        }
    }
}
