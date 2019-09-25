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
    public Vector3 location;

    public PortalState Next()
    {
        PortalState[] portalStates = (PortalState[])Enum.GetValues(portalState.GetType());
        int j = Array.IndexOf<PortalState>(portalStates, portalState) + 1;
        return (portalStates.Length == j) ? portalStates[0] : portalStates[j];
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        //if(player != null) player.Teleport(location);
    }
}
