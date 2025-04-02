using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Transform target;
    public PlayerController player;
    public Animator enemyAnimator;

    [Header("Properties")]
    public string enemyName;
    public int modelNumber;
    public bool jumpScareAsset = false;
    private JumpScareMechanic jumpScareMechanic;

    [Header("Player Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    public bool playerInSight;
    private bool previousPlayerInSight = false;

    [Header("Chase Function Settings")]
    public bool chasingPlayer = false;
    public bool timerSet;
    private float baseChasingTimer = 6f;
    private float currentChasingTimer;

    [Header("Speed Settings")]
    public float defaultSpeed = 3.5f;
    public float chaseSpeed = 6f;

    [Header("Patrolling Settings")]
    public float patrolRadius = 10f;
    public float patrolWaitTime = 3f;
    private Vector3 randomDestination;
    private bool isPatrolling = true;

    void Start()
    {
        if (!jumpScareAsset)
        {
            AssignComponents();
            StartCoroutine(Patrol());
        }

        // Get the JumpScareMechanic instance in the scene
        jumpScareMechanic = FindObjectOfType<JumpScareMechanic>();
    }

    void Update()
    {
        if (!jumpScareAsset && target != null)
        {
            SearchForPlayer();

            if (playerInSight)
            {
                agent.SetDestination(player.transform.position);
                DetectionMeter();
            }
            else
            {
                chasingPlayer = false;
                timerSet = false;
                agent.speed = defaultSpeed;

                if (!isPatrolling)
                {
                    StartCoroutine(Patrol());
                }
            }
        }

        if (enemyAnimator != null)
        {
            UpdateAnimations();
        }
    }

    IEnumerator Patrol()
    {
        isPatrolling = true;

        while (!playerInSight)
        {
            randomDestination = GetRandomPoint(transform.position, patrolRadius);
            agent.SetDestination(randomDestination);

            yield return new WaitUntil(() => agent.remainingDistance < 0.5f);

            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetBool("Idle", true);

            yield return new WaitForSeconds(patrolWaitTime);

            enemyAnimator.SetBool("Walk", true);
            enemyAnimator.SetBool("Idle", false);
        }

        isPatrolling = false;
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPos = center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center;
    }

    public void AssignComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("There is no NavMeshAgent component on " + gameObject.name + "!");
        }
        else
        {
            agent.speed = defaultSpeed;
        }

        if (player == null)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();

            foreach (PlayerController p in players)
            {
                if (p.mainCharacter)
                {
                    player = p;
                    playerLayer = LayerMask.GetMask(LayerMask.LayerToName(player.gameObject.layer));
                    break;
                }
            }

            if (player == null)
            {
                Debug.LogError("No PlayerController found in the scene.");
            }
        }

        previousPlayerInSight = false;
    }

    public void SearchForPlayer()
    {
        if (player == null || !player.isActiveCharacter) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        playerInSight = false;

        foreach (Collider hit in hits)
        {
            if (hit.transform == player.transform)
            {
                if (hit.TryGetComponent(out HidingMechanic hiding) && hiding.isHidden)
                {
                    Debug.Log("Player is hidden. Not detected.");
                    continue;
                }

                playerInSight = true;
                Debug.Log("Player detected!");
                break;
            }
        }

        if (!playerInSight)
        {
            Debug.Log("No player detected.");
        }

        previousPlayerInSight = playerInSight;
    }

    public void DetectionMeter()
    {
        if (!chasingPlayer && playerInSight)
        {
            chasingPlayer = true;
            agent.speed = chaseSpeed;

            Debug.Log("Chasing player!");

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Chase");
            }
        }
        else if (chasingPlayer && !playerInSight)
        {
            chasingPlayer = false;
            agent.speed = defaultSpeed;

            Debug.Log("Player lost! Stopping chase.");

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Idle");
            }
        }
    }

    public void UpdateAnimations()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            enemyAnimator.SetBool("Walk", true);
            enemyAnimator.SetBool("Idle", false);
        }
        else
        {
            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetBool("Idle", true);
        }

        if (chasingPlayer && enemyAnimator != null)
        {
            enemyAnimator.SetBool("Chase", true);
        }
        else
        {
            enemyAnimator.SetBool("Chase", false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy touched the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy touched the player!");

            // Trigger the jump scare when the enemy touches the player
            if (jumpScareMechanic != null)
            {
                jumpScareMechanic.CreateJumpScare(this, modelNumber, collision.gameObject);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
