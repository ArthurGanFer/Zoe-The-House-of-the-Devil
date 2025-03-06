using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip ("A reference to our NavMeshAgent component")]
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
    private LayerMask playerLayer;      //The layer the player is on
    [SerializeField]
    [Tooltip("A flag for if the player is in vision")]
    private bool playerInSight;         //A flag for if the player is in vision

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
    }

    void IsPlayerInSight()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, this.detectionRadius, this.playerLayer);

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
                    else
                    {
                        this.playerInSight = false;
                    }
                }
                else
                {
                    this.playerInSight = true;
                }
                
                return;
            }
        }

        playerInSight = false; 
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

}
