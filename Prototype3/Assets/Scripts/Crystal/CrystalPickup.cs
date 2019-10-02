using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CrystalType
{
    None,
    Red,
    Yellow,
    Blue,
    Purple,
}

public class CrystalPickup : MonoBehaviour
{
    public CrystalHolder currentHolder;
    public CrystalType type = CrystalType.Blue;
    public float grabTime = 1;

    private void Start()
    {
        Debug.Assert(currentHolder, "Holder is null. Did you accidentally put a crystal in the scene? Only pedestals should spawn crystals.", this);
        Debug.Assert(type != CrystalType.None, nameof(CrystalType) + " is None. Did you forget to use a prefab variant?", this);
    }

    public void OnPickedUp(CrystalHolder holder)
    {
        currentHolder = holder;
        transform.SetParent(holder.pos, true);
        transform.DOKill();
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(Vector3.zero, grabTime));
        seq.Insert(0, transform.DOLocalRotate(Vector3.zero, grabTime));
    }
}
