using UnityEngine;
using UnityEngine.AI;

public enum FriendState
{
    WaitingForFriend,
    Moving,
    AtHangi
}

public class Friend : MonoBehaviour
{
    public FriendState state;
    public NavMeshAgent agent;
    public Transform[] path;

    public bool launched = false;

    public float arriveRadius = 1.0f;

    private int currentPathPoint = 0;
    private Rigidbody rigidBody;

    private void Start()
    {
        agent.isStopped = true;
        agent.SetDestination(path[currentPathPoint].position);

        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (launched)
        {
            if (rigidBody.velocity.magnitude < 1.0f)
            {
                launched = false;
                rigidBody.isKinematic = true;
                agent.enabled = true;
                agent.isStopped = false;
                agent.SetDestination(path[currentPathPoint].position);
            }
        }

        switch(state)
        {
            case FriendState.WaitingForFriend:
                {
                    
                    break;
                }

            case FriendState.Moving:
                {
                    // agent.isStopped = false;
                    CheckPathPoint();
                    break;
                }

            case FriendState.AtHangi:
                {
                    agent.isStopped = true;
                    break;
                }

            default: break;
        }
    }

    void MoveToHangi()
    {
        state = FriendState.Moving;
        agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (path.Length == 0) { return; }
        for (int i = 0; i < path.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(path[i].position, arriveRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == FriendState.AtHangi) { return; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MoveToHangi();
        }
    }

    private void CheckPathPoint()
    {
        if ((this.transform.position - path[currentPathPoint].position).magnitude < arriveRadius)
        {
            NextPoint();
        }
    }

    private void NextPoint()
    {
        if (currentPathPoint >= path.Length - 1) {state = FriendState.AtHangi; return; }
        agent.SetDestination(path[++currentPathPoint].position);
    }

    public void Launch()
    {
        agent.enabled = false;
        launched = true;
        rigidBody.isKinematic = false;
    }
}
