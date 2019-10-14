using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraView : MonoBehaviour
{

    private CinemachineVirtualCamera cam;
    private Player playerRef;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.enabled = false;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ActivateFor(float seconds)
    {
        cam.enabled = true;

        playerRef.SetInputsDisabled(true);
        transform.DOMove(this.transform.position, seconds).OnComplete(Deactivate);
    }

    private void Deactivate()
    {
        cam.enabled = false;
        playerRef.SetInputsDisabled(false);
    }
}
