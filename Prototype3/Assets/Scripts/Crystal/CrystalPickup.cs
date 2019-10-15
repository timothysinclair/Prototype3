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
    //private CrystalHolder previousHolder = null;
    private CrystalHolder originalHolder = null;


    private void Start()
    {
        Debug.Assert(currentHolder, "Holder is null. Did you accidentally put a crystal in the scene? Only pedestals should spawn crystals.", this);
        Debug.Assert(type != CrystalType.None, nameof(CrystalType) + " is None. Did you forget to use a prefab variant?", this);
        originalHolder = currentHolder;
    }

    public void OnPickedUp(CrystalHolder holder)
    {
        // previousHolder = currentHolder;
        currentHolder = holder;
        transform.SetParent(holder.pos, true);
        transform.DOKill();
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(Vector3.zero, grabTime));
        seq.Insert(0, transform.DOLocalRotate(Vector3.zero, grabTime));
        seq.Append(transform.DORotate(new Vector3(0.0f, 90.0f, 0.0f), 0.5f));
    }

    public void ReturnToOriginalHolder()
    {
        if (originalHolder.held)
        {
            originalHolder.held.ReturnToOriginalHolder();
        }

        OnPickedUp(originalHolder);
    }
}
