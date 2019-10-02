using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalHolder : MonoBehaviour
{
    public Transform pos;
    public List<CrystalType> allowedTypes = new List<CrystalType> { CrystalType.None, CrystalType.Blue, CrystalType.Yellow, CrystalType.Purple, CrystalType.Red };
    public CrystalPickup initialPrefab;

    public CrystalPickup held;

    public event Action<CrystalType> onPickedUp;

    public CrystalType heldType { get { return held?.type ?? CrystalType.None; } }

    public bool IsAllowedType(CrystalType type) { return allowedTypes.Contains(type); }

    private void Awake()
    {
        if (held)
        {
            PickItUp(held);
        }
        else if (initialPrefab)
        {
            if (IsAllowedType(initialPrefab.type))
            {
                var pickup = Instantiate(initialPrefab, transform.position, transform.rotation);
                PickItUp(pickup);
                initialPrefab = null;
            }
            else
            {
                Debug.LogError("initial prefab's crystal type is not allowed for this holder", this);
            }
        }
    }

    private void PickItUp(CrystalPickup pickup)
    {
        if (!pickup)
        {
            held = null;
            onPickedUp?.Invoke(CrystalType.None);
            return;
        }
        held = pickup;
        pickup.OnPickedUp(this);
        onPickedUp?.Invoke(pickup.type);
    }

    public void Swap(CrystalHolder other)
    {
        var a = other.IsAllowedType(this.heldType);
        var b = this.IsAllowedType(other.heldType);
        if (a && b)
        {
            var pickup = other.held;
            other.PickItUp(this.held);
            this.PickItUp(pickup);
        }
    }
}
