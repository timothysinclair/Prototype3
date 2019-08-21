using UnityEngine;
using UnityEngine.AI;

public enum FriendState
{
    WaitingToTalk,
    WaitingForFood,
    Moving,
    AtHangi
}

public class Friend : MonoBehaviour
{
    public FriendState state;
    public NavMeshAgent agent;
    public Transform[] path;

    public bool launched = false;
    public GameObject questFood;
    public AudioClip talkingSound;
    private AudioSource audioSource;

    public float arriveRadius = 1.0f;

    private int currentPathPoint = 0;
    private Rigidbody rigidBody;
    public int friendIndex;
    public FoodType foodType;

    public Animator friendAnimator;

    private void Start()
    {
        agent.isStopped = true;
        agent.SetDestination(path[currentPathPoint].position);

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (launched)
        {
            if ((rigidBody.velocity.magnitude < 1.0f) && !(rigidBody.velocity.y > 0.0f))
            {
                launched = false;
                rigidBody.isKinematic = true;
                agent.enabled = true;
                agent.isStopped = false;
                agent.SetDestination(path[currentPathPoint].position);
                if (friendAnimator) { friendAnimator.speed = 1.0f; }
            }
        }

        switch(state)
        {
            case FriendState.WaitingToTalk:
                {
                    
                    break;
                }

            case FriendState.WaitingForFood:
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

    public void Talk()
    {
        if (state == FriendState.WaitingToTalk)
        {
            var playerInv = FindObjectOfType<PlayerInventory>();
            if (playerInv)
            {
                if (playerInv.QueryFood(foodType))
                {
                    PlayTalkingSound();
                    PlayerUI.Instance.CreateTempTextbox(FriendData.leavingMessages[friendIndex]);
                    MoveToHangi();
                }
                else
                {
                    PlayTalkingSound();
                    PlayerUI.Instance.CreateTempTextbox(FriendData.greetingMessages[friendIndex]);
                    state = FriendState.WaitingForFood;

                    questFood.SetActive(true);
                }
            }
         
        }
        else if (state == FriendState.WaitingForFood)
        {
            var playerInv = FindObjectOfType<PlayerInventory>();
            if (playerInv)
            {
                if (playerInv.QueryFood(foodType))
                {
                    PlayTalkingSound();
                    PlayerUI.Instance.CreateTempTextbox(FriendData.leavingMessages[friendIndex]);
                    MoveToHangi();
                }
                else
                {
                    PlayTalkingSound();
                    PlayerUI.Instance.CreateTempTextbox(FriendData.greetingMessages[friendIndex]);
                }
                
            }
            
        }
    }

    public void MoveToHangi()
    {
        state = FriendState.Moving;
        agent.isStopped = false;
        if (friendAnimator) { friendAnimator.SetBool("Running", true); }
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
        if (state == FriendState.Moving) { return; }

        var player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            player.SetActionState(ActionState.talk);
            player.SetFriend(GetComponent<Friend>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (state == FriendState.AtHangi) { return; }
        var player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            player.SetActionState(ActionState.jump);
            player.SetFriend(null);
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
        if (state == FriendState.AtHangi) { return; }
        if (currentPathPoint >= path.Length - 1)
        {
            GameManager.Instance.FriendArrived();
            state = FriendState.AtHangi;
            if (friendAnimator) { friendAnimator.SetBool("Running", false); }
            return;
        }
        agent.SetDestination(path[++currentPathPoint].position);
    }

    public void Launch()
    {
        agent.enabled = false;
        launched = true;
        rigidBody.isKinematic = false;
    }

    public void PlayTalkingSound()
    {
        switch (friendIndex)
        {
            case 0:
                {
                    audioSource.PlayOneShot(talkingSound);
                    break;
                }

            case 1:
                {
                    audioSource.PlayOneShot(talkingSound);
                    break;
                }

            case 2:
                {
                    audioSource.PlayOneShot(talkingSound);
                    break;
                }
        }
    }
}
