using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private UnityEvent onLevelBegin;
    [SerializeField] private UnityEvent onLevelEnd;
    [SerializeField] private UnityEvent onMoveEnd;
    [SerializeField] private UnityEvent onTwoSecondsAfterMoveEnd;

    [SerializeField] private Puzzle[] levelPuzzles;

    [Tooltip("The position this level will move to after being completed")]
    [SerializeField] private Transform endTransform;
    [SerializeField] private float moveTime = 3.0f;

    private Vector3 endPosition;
    private Player playerRef;
    private Transform oldPlayerRootTransform;

    private Rigidbody rigidBody;
    private int currentPuzzleIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        endPosition = endTransform.position;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        DOTween.defaultUpdateType = UpdateType.Fixed;
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToEndPosition()
    {
        playerRef.GetRootTransform().SetParent(this.transform);
        rigidBody.transform.DOMove(endPosition, moveTime).SetEase(Ease.InOutSine).OnComplete(ResetPlayerParentTransform).OnComplete(MoveEnd);
    }

    private void MoveEnd()
    {
        onMoveEnd.Invoke();
        rigidBody.transform.DOMove(this.transform.position, 2.0f).OnComplete(onTwoSecondsAfterMoveEnd.Invoke);
    }

    private void ResetPlayerParentTransform()
    {
        playerRef.GetRootTransform().SetParent(null);
    }

    public Puzzle GetCurrentPuzzle()
    {
        return levelPuzzles[currentPuzzleIndex];
    }
}
