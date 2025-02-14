using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private NavMeshAgent agent;         //A reference to our NavMeshAgent component
    [SerializeField]
    private Transform target;           //A reference to our target GameObject's Transform component
    [SerializeField]
    private Transform player;           //A reference to our player GameObject's Transform component

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    public string enemyName;
    [SerializeField]
    public int modelNumber;
    [SerializeField]
    private bool jumpScareAsset;
    [SerializeField]
    private float lookAheadDistance;    //The Boss' distance of vision 
    [SerializeField]
    private float lookAheadAngle;       //The Boss' range of vision
    [SerializeField]
    private LayerMask playerLayer;      //The layer the player is on
    [SerializeField]
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
        float detectionRadius = this.lookAheadDistance;

        // Get all colliders in the detection radius
        Collider[] hits = Physics.OverlapSphere(this.transform.position, detectionRadius, this.playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && hit.GetComponent<Transform>() == this.player)
            {
                if (!hit.GetComponent<HidingMechanic>().isHidden)
                {
                    this.playerInSight = true;
                }
                else
                {
                    this.playerInSight = false;
                }
                
                return;
            }

            /*
            if (hit.CompareTag("Player") && hit.GetComponent<PlayerController>().Main_Character != false)
            {
                if (hit.GetComponent<HidingMechanic>() != null && hit.GetComponent<HidingMechanic>().isHidden == true)
                {
                    Debug.Log("Error");
                    return;
                }

                Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                if (angleToPlayer <= lookAheadAngle / 2)
                {
                    playerInSight = true;
                    return; // Player is detected, stop checking
                }
            }
            */
        }

        playerInSight = false; // Player is not in sight
    }

    private void OnDrawGizmosSelected()
    {
        if (this.playerInSight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.lookAheadDistance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.lookAheadDistance);
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = playerInSight ? Color.green : Color.red;

        float halfAngle = lookAheadAngle / 2;
        Vector3 forward = transform.forward;
        float[] heights = { 2.0f, 1.0f, 0.5f, 0.1f }; // Match the heights used in your logic

        foreach (float height in heights)
        {
            Vector3 origin = transform.position + Vector3.up * height;

            for (float angle = -halfAngle; angle <= halfAngle; angle += 5.0f) // Use larger steps for visualization
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
                Gizmos.DrawRay(origin, direction * lookAheadDistance);
            }
        }
    }
    */


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Transform>() == this.player)
        {
            Debug.Log($"Player touched by {enemyName}! Game Over!");

            JumpScareMechanic jumpScareMechanic = FindObjectOfType<JumpScareMechanic>();
            jumpScareMechanic.CreateJumpScare(this, this.modelNumber, collision.gameObject);

            //SceneManager.LoadScene("Game_Scene");
        }
    }

}
