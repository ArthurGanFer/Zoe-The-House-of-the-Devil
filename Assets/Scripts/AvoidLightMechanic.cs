using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvoidLightMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private LightMechanic lightMechanic;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private NavMeshAgent agent;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    private bool lightSeen;
    [SerializeField]
    private float speedDelta;



    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightSeen)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = speedDelta;
        }
    }

    private void AssignComponents()
    {
        lightMechanic = FindObjectOfType<LightMechanic>();
        if (lightMechanic == null)
        {
            Debug.Log($"There is no GameObject of Type LightMechanic in this scene!");
        }
        col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.Log($"There is no Collider Component on {this} GameObject!");
        }
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.Log($"There is no NavMeshAgent Component on {this} GameObject!");
        }

        speedDelta = agent.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == lightMechanic.lightCol)
        {
            lightSeen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lightSeen = false;
    }
}
