using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerControllerRigidbody playerController;
    [SerializeField] private PlayerInputs playerInput;

    private void Start()
    {
        playerInput.ToggleCursorLock();
    }

    public bool GetInputsDisabled()
    {
        return playerInput.GetInputsDisabled();
    }

    public void SetInputsDisabled(bool disabled)
    {
        playerInput.SetInputsDisabled(disabled);
    }

    public bool IsCamouflaged()
    {
        return playerController.IsCamouflaged();
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        playerController.TeleportPlayer(newPosition);
    }
}
