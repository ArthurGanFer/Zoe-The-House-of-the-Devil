using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class hookMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private grappleMechanic grappleMechanic;
    [SerializeField]
    private Rigidbody hookRB;
    [SerializeField]
    private LineRenderer hookLR;
    public List<Transform> hookLocs = new List<Transform>();

    [Space(10)]

    [Header("Properties")]
    [SerializeField]
    private LayerMask grapplePoint;
    [SerializeField]
    private float hookForce = 25f;
    [SerializeField]
    private bool foundTarget;
    [SerializeField]
    private float searchRadius = 5000;
    [SerializeField]
    private float startTime;



    public void InitializeGrapple(grappleMechanic grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grappleMechanic = grapple;
        AssignComponents();
        hookRB.AddForce(transform.forward * hookForce, ForceMode.Impulse);

        startTime = Time.time;
    }

    private void AssignComponents()
    {
        hookRB = GetComponent<Rigidbody>();
        if (hookRB == null )
        {
            Debug.Log($"There is no Rigidbody Component on {this} GameObject!");
        }
        hookLR = GetComponent<LineRenderer>();
        if (hookLR == null)
        {
            Debug.Log($"There is no LineRenderer Component on {this} GameObject!");
        }
    }

    private void Update()
    {
        FindTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((grapplePoint & 1 << other.gameObject.layer) > 0)
        {
            hookRB.useGravity = false;
            hookRB.isKinematic = true;

            grappleMechanic.StartPull();

            Debug.Log($"Collided with {other.gameObject}");
        }
    }

    public void FindTarget()
    {
        hookLocs.Clear();

        Collider[] hooksCol = Physics.OverlapSphere(transform.position, searchRadius, grapplePoint);

        for (int i = 0; i < hooksCol.Length; i++)
        {
            hookLocs.Add(hooksCol[i].transform);
        }

        if (hookLocs.Count > 0)
        {
            foundTarget = true;
        }
        else
        {
            foundTarget = false;
        }

        Vector3[] positions = new Vector3[] {hookLocs[0].transform.position, grappleMechanic.transform.position};

        hookLR.SetPositions(positions);

        MoveToGrapplePoint();
    }

    private void MoveToGrapplePoint()
    {
        float journeyLength = Vector3.Distance(grappleMechanic.shootTransform.position, hookLocs[0].position);

        float distCovered = (Time.time - startTime) * hookRB.velocity.magnitude;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(grappleMechanic.shootTransform.position, hookLocs[0].position, fractionOfJourney);
    }

    private void OnDrawGizmosSelected()
    {
        if (foundTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }
}
