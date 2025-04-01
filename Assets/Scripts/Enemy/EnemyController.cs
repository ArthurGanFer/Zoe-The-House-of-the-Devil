using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("A reference to our NavMeshAgent component")]
    public NavMeshAgent agent;
    [Tooltip("A reference to our target GameObject's Transform component")]
    public Transform target;
    [Tooltip("A reference to the player's PlayerController component")]
    public PlayerController player;
    [Tooltip("A reference to the our Animator component")]
    public Animator enemyAnimator;

    [Space(10)]
    [Header("Properties")]
    [Space(1)]
    [Header("Detail Settings")]
    [SerializeField]
    [Tooltip("A string reflecting who our enemy is")]
    public string enemyName;
    [SerializeField]
    [Tooltip("An int reflecting who our enemy is")]
    public int modelNumber;
    [SerializeField]
    [Tooltip("A flag for if this enemy is just a jump scare asset")]
    public bool jumpScareAsset = false;

    [Space(1)]
    [Header("Player Detection Settings")]
    [SerializeField]
    [Tooltip("Our enemy's range of vision")]
    private float detectionRadius = 1;
    [SerializeField]
    [Tooltip("The LayerMask representing where our character is located")]
    private LayerMask playerLayer;
    [Tooltip("A flag for if the player is in vision")]
    public bool playerInSight;
    [Tooltip("A flag that tracks the previous state of playerInSight")]
    [SerializeField]
    private bool previousPlayerInSight = false;

    [Space(1)]
    [Header("Chase Function Settings")]
    [Tooltip("A flag for if we are chasing the player")]
    public bool chasingPlayer = false;
    [Tooltip("A flag representing if the current timer has been set")]
    public bool timerSet;
    [SerializeField]
    [Tooltip("A float for how long we wait before chasing the player")]
    private float baseChasingTimer = 6;
    [SerializeField]
    [Tooltip("A float for how long we currently have before chasing the player")]
    private float currentChasingTimer;

    [Space(1)]
    [Header("Speed Settings")]
    [Tooltip("The default speed of the enemy")]
    public float defaultSpeed = 3.5f;
    [SerializeField]
    [Tooltip("The speed of the enemy when chasing the player")]
    private float chaseSpeed = 6f;


    // Start is called before the first frame update
    void Start()
    {
        if (!this.jumpScareAsset)
        {
            AssignComponents();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.jumpScareAsset && this.target != null)
        {
            SearchForPlayer();

            if (this.playerInSight)
            {
                DetectionMeter();
            }
            else
            {
                this.chasingPlayer = false;
                this.timerSet = false;
                this.agent.speed = this.defaultSpeed;
                this.agent.SetDestination(this.target.position);
            }
        }

        if (this.enemyAnimator != null)
        {
            UpdateAnimations();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Transform>() == this.player.GetComponent<Transform>())
        {
            Debug.Log($"Player touched by {enemyName}! Game Over!");

            JumpScareMechanic jumpScareMechanic = FindObjectOfType<JumpScareMechanic>();
            jumpScareMechanic.CreateJumpScare(this, this.modelNumber, collision.gameObject);
        }
    }

    public void AssignComponents()
    {
        this.agent = GetComponent<NavMeshAgent>();
        if (this.agent == null)
        {
            Debug.Log($"There is no NavMeshAgent component on {this.gameObject.name} gameObject!");
        }
        else
        {
            this.agent.speed = this.defaultSpeed;
        }

        if (this.jumpScareAsset)
        {
            if (this.target == null)
            {
                Debug.LogError($"There is no Transform component on the child of {this.gameObject.name} gameObject!");
            }
        }

        if (this.player == null)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();

            foreach (PlayerController player in players)
            {
                if (player.mainCharacter == true)
                {
                    this.player = player;

                    string playerLayerName = LayerMask.LayerToName(this.player.gameObject.layer);
                    this.playerLayer = LayerMask.GetMask(playerLayerName);

                    break;
                }
            }

            Debug.Log($"There are no gameObjects of type PlayerController in scene: {SceneManager.GetActiveScene().name}");
        }

        this.previousPlayerInSight = false;
    }

    public void DetectionMeter()
    {
        if (!this.chasingPlayer && this.player.is_crouching)
        {
            this.StartCoroutine("StartDetectionTimer", baseChasingTimer);
        }
        else
        {
            this.StopCoroutine("StartDetectionTimer");
            this.chasingPlayer = true;
            this.agent.SetDestination(this.player.GetComponent<Transform>().position);
            this.timerSet = false;
            this.agent.speed = chaseSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (this.playerInSight && this.chasingPlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.detectionRadius);
        }
        else if (this.playerInSight && !this.chasingPlayer)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, this.detectionRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.detectionRadius);
        }
    }

    public void SearchForPlayer()
    {
        if (this.player.isActiveCharacter)
        {
            Collider[] hits = Physics.OverlapSphere(this.transform.position, this.detectionRadius, this.playerLayer);

            this.playerInSight = false;

            foreach (Collider hit in hits)
            {
                if (hit.GetComponent<Transform>() == this.player.GetComponent<Transform>())
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

            this.previousPlayerInSight = this.playerInSight;
        }
        else
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();

            foreach (PlayerController player in players)
            {
                if (player.isActiveCharacter == true)
                {
                    this.player = player;

                    break;
                }
            }
        }
    }

    public void UpdateAnimations()
    {
        if (this.agent.velocity.magnitude > 0)
        {
            this.enemyAnimator.SetBool("Walking", true);
            this.enemyAnimator.SetBool("Idle", false);
        }
        else
        {
            this.enemyAnimator.SetBool("Walking", false);
            this.enemyAnimator.SetBool("Idle", true);
        }

        if (this.playerInSight && !this.previousPlayerInSight)
        {
            enemyAnimator.SetTrigger("Chase");
        }
        else if (!this.playerInSight && this.previousPlayerInSight)
        {
            enemyAnimator.SetTrigger("Chase");
        }
    }

    IEnumerator StartDetectionTimer(float baseTimer)
    {
        if (!this.timerSet)
        {
            this.currentChasingTimer = baseTimer;
            this.timerSet = true;
        }
        else
        {
            this.currentChasingTimer -= Time.deltaTime;

            if (this.currentChasingTimer <= 0)
            {
                yield return this.chasingPlayer = true;
            }
        }
    }
}
