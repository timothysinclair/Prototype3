using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpinAnim : MonoBehaviour
{
    public float duration = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() => transform.localEulerAngles = Vector3.zero)
            .SetLoops(-1);
    }
}
