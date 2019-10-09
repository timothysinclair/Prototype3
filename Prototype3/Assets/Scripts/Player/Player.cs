using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControllerRigidbody))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(CrystalHolder))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerControllerRigidbody playerController;
    [SerializeField] private PlayerInputs playerInput;
    [SerializeField] private CrystalHolder playerCrystalHolder;

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

    public void ReturnCrystal()
    {
        playerCrystalHolder.ReturnHeldToOriginal();
    }

    public CrystalType HeldCrystalType()
    {
        if (!playerCrystalHolder.held) return CrystalType.None;
        return playerCrystalHolder.held.type;
    }

    public PlayerInputs GetInputs()
    {
        return playerInput;
    }
}
