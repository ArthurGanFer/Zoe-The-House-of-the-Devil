using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;         //A reference to our NavMeshAgent component
    [SerializeField]
    private Transform target;           //A reference to our target GameObject's Transform component
    [SerializeField]
    private Transform player;           //A reference to our player GameObject's Transform component

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
        if (player != null && target != null)
        {
            IsPlayerInSight();

            if (playerInSight)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.SetDestination(target.position);
            }
        } 
    }

    void IsPlayerInSight()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * lookAheadDistance;
        float halfAngle = lookAheadAngle / 2;
        
        for (float angle = -halfAngle; angle <= halfAngle; angle += 1.0f)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, lookAheadDistance, playerLayer))
            {
                if (hit.collider != null)
                {
                    playerInSight = true;
                    break;
                }
            }
            else
            {
                playerInSight = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerInSight)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Vector3 forward = transform.forward;
        float halfAngle = lookAheadAngle / 2;

        for (float angle = -halfAngle; angle <= halfAngle; angle += 1.0f)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward; 
            Gizmos.DrawRay(transform.position, direction * lookAheadDistance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Gets Here");

        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndScreen");
            Debug.Log("Player Dead!");
        }
    }
}
