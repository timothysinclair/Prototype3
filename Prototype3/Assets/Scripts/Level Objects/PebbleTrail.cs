using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PebbleTrail : MonoBehaviour
{
    public Material onMat;
    public Material offMat;

    public MeshRenderer[] renderers;

    private void Start()
    {
        //foreach (MeshRenderer renderer in renderers)
        //{
        //    renderer.material = offMat;
        //}
    }

    public void OnPowered()
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = onMat;
        }
    }

    public void OnPowerLoss()
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = offMat;
        }
    }
}
