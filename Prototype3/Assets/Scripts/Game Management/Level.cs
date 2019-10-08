using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [SerializeField] private UnityEvent onLevelBegin;
    [SerializeField] private UnityEvent onLevelEnd;

    [SerializeField] private Puzzle[] levelPuzzles;

    [Tooltip("The position this level will move to after being completed")]
    [SerializeField] private Transform endTransform;
    [SerializeField] private float moveTime = 3.0f;

    private Vector3 endPosition;
    private Player playerRef;
    private Transform oldPlayerRootTransform;

    // Start is called before the first frame update
    void Start()
    {
        endPosition = endTransform.position;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToEndPosition()
    {
        playerRef.GetRootTransform().SetParent(this.transform);
        transform.DOMove(endPosition, moveTime).SetEase(Ease.InOutSine).OnComplete(ResetPlayerParentTransform);
    }

    private void ResetPlayerParentTransform()
    {
        playerRef.GetRootTransform().SetParent(null);
    }
}
