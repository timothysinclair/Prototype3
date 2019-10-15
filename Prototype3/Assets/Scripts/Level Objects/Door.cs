using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [Tooltip("The types of crystals that the player can take through this doorway")]
    [SerializeField] private List<CrystalType> allowedTypes = new List<CrystalType> { CrystalType.None, CrystalType.Blue, CrystalType.Yellow, CrystalType.Purple, CrystalType.Red };

    [Tooltip("Solid doors are opened when powered, otherwise they are just barriers for crystals")]
    [SerializeField] private bool isSolid = false;

    [SerializeField] private Material poweredMaterial;
    [SerializeField] private Material unpoweredMaterial;

    // private Collider myCollider;
    private MeshRenderer myMesh;

    private void Awake()
    {
        // myCollider = GetComponent<Collider>();
        myMesh = GetComponent<MeshRenderer>();

        // if (!isSolid) { myCollider.isTrigger = true; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (!allowedTypes.Contains(player.HeldCrystalType()))
            {
                player.ReturnCrystal();
            }
        }
    }

    public void OnPowered()
    {
        if (isSolid)
        {
            // myCollider.isTrigger = true;
        }
        myMesh.material = poweredMaterial;
    }

    public void OnPowerLoss()
    {
        if (isSolid)
        {
            // myCollider.isTrigger = false;
        }
        myMesh.material = unpoweredMaterial;
    }
}
