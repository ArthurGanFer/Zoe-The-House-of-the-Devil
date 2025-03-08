using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("A reference to our NavMeshAgent component")]
    private NavMeshAgent agent;

    [SerializeField]
    [Tooltip("A reference to our target GameObject's Transform component")]
    private Transform target;

    [SerializeField]
    [Tooltip("A reference to our player GameObject's Transform component")]
    private Transform player;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    [Tooltip("A string reflecting who our enemy is")]
    public string enemyName;

    [SerializeField]
    [Tooltip("An int reflecting who our enemy is")]
    public int modelNumber;

    [SerializeField]
    [Tooltip("A flag for if this enemy is just a jump scare asset")]
    private bool jumpScareAsset;

    [SerializeField]
    [Tooltip("Our enemy's range of vision")]
    private float detectionRadius;

    [SerializeField]
    [Tooltip("The LayerMask representing where our character is located")]
    private LayerMask playerLayer; // The layer the player is on

    [SerializeField]
    [Tooltip("A flag for if the player is in vision")]
    private bool playerInSight; // A flag for if the player is in vision

    [Header("Speed Settings")]
    [SerializeField]
    [Tooltip("The default speed of the enemy")]
    private float defaultSpeed = 3.5f;

    [SerializeField]
    [Tooltip("The speed of the enemy when chasing the player")]
    private float chaseSpeed = 6f;

    [Header("Animation")]
    [SerializeField]
    [Tooltip("A reference to the enemy's Animator component")]
    private Animator enemyAnimator;

    private bool previousPlayerInSight; // Track the previous state of playerInSight

    private void Start()
    {
        // Initialize the agent's speed to the default speed
        agent.speed = defaultSpeed;
        previousPlayerInSight = false; // Initialize the previous state
    }

    private void Update()
    {
        if (!this.jumpScareAsset)
        {
            if (this.player != null && this.player.GetComponent<PlayerController>().isActiveCharacter && this.target != null)
            {
                IsPlayerInSight();

                if (this.playerInSight)
                {
                    this.agent.SetDestination(this.player.position);
                }
                else
                {
                    this.agent.SetDestination(this.target.position);
                }
            }
            else
            {
                this.target = GetComponentInChildren<Transform>();

                PlayerController[] availablePlayers = FindObjectsOfType<PlayerController>();

                foreach (PlayerController availablePlayer in availablePlayers)
                {
                    if (availablePlayer.isActiveCharacter)
                    {
                        this.player = availablePlayer.GetComponent<Transform>();
                        break;
                    }
                }
            }
        }

        // Update walking and idle animations
        if (enemyAnimator != null)
        {
            if (this.agent.velocity.magnitude > 0)
            {
                enemyAnimator.SetBool("Walk", true);
                enemyAnimator.SetBool("Idle", false);
            }
            else
            {
                enemyAnimator.SetBool("Walk", false);
                enemyAnimator.SetBool("Idle", true);
            }
        }
    }

    void IsPlayerInSight()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, this.detectionRadius, this.playerLayer);

        this.playerInSight = false; // Assume player is not in sight initially

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && hit.GetComponent<Transform>() == this.player)
            {
                if (hit.GetComponent<HidingMechanic>() != null)
                {
                    if (!hit.GetComponent<HidingMechanic>().isHidden)
                    {
                        this.playerInSight = true;
                    }
                }
                else
                {
                    this.playerInSight = true;
                }

                break; 
            }
        }

        if (this.playerInSight)
        {
            agent.speed = chaseSpeed; 
        }
        else
        {
            agent.speed = defaultSpeed; 
        }

        if (this.playerInSight && !previousPlayerInSight)
        {
            StartChaseAnimation();
        }
        else if (!this.playerInSight && previousPlayerInSight)
        {
            ResetChaseTrigger();
        }

        previousPlayerInSight = this.playerInSight;
    }

    private void OnDrawGizmosSelected()
    {
        if (this.playerInSight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.detectionRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.detectionRadius);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Transform>() == this.player)
        {
            Debug.Log($"Player touched by {enemyName}! Game Over!");

            JumpScareMechanic jumpScareMechanic = FindObjectOfType<JumpScareMechanic>();
            jumpScareMechanic.CreateJumpScare(this, this.modelNumber, collision.gameObject);
        }
    }

    private void StartChaseAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Chase");
        }
    }

    private void ResetChaseTrigger()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Chase");
        }
    }
}