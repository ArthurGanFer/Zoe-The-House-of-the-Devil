using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (player != null && player.GetComponent<PlayerController>().Main_Character && target != null)
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
        else
        {
            PlayerController[] availablePlayers = FindObjectsOfType<PlayerController>();
          
            foreach (PlayerController availablePlayer in availablePlayers)
            {
                if (availablePlayer.Main_Character)
                {
                    player = availablePlayer.GetComponent<Transform>();

                    break;
                }
            }
        }
    }

    void IsPlayerInSight()
    {
        float detectionRadius = lookAheadDistance;

        // Get all colliders in the detection radius
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && hit.GetComponent<Transform>() == player)
            {
                if (!hit.GetComponent<HidingMechanic>().isHidden)
                {
                    playerInSight = true;
                }
                else
                {
                    playerInSight = false;
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
        if (playerInSight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lookAheadDistance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookAheadDistance);
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


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Player touched by boss! Game Over!");

    //        SceneManager.LoadScene("Game_Scene");
    //    }
    //}

}
