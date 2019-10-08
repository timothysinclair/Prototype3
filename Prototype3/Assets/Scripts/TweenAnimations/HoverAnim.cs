using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoverAnim : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float duration = 1;
    private float phasor = 0;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.To(
            () => phasor,
            v => {
                phasor = v;
                transform.localPosition = new Vector3(0, amplitude * Mathf.Sin(phasor * Mathf.Deg2Rad), 0);
            },
            360,
            duration
            )
            .SetEase(Ease.Linear)
            .OnComplete(() => phasor = 0)
            .SetLoops(-1);
    }
}
