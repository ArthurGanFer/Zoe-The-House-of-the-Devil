using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaGrappleMechanic : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField]
    [Tooltip ("A list of Transforms representing possible grapple points")]
    private List<Transform> hookLocs = new List<Transform>();
    [SerializeField]
    [Tooltip("Our target grapple point")]
    private Transform target;
    [SerializeField]
    [Tooltip ("A reference to our RigidBody")]
    private Rigidbody rigidBody;
    [SerializeField]
    [Tooltip("A reference to our LineRenderer")]
    private LineRenderer lineRenderer;

    [Space(10)]

    [Header("Properties")]
    [SerializeField]
    [Tooltip ("A flag for if there is a possible grapple target")]
    private bool foundTarget;
    [SerializeField]
    [Tooltip ("The LayerMask where we keep our grapple points")]
    private LayerMask grapplePoint;
    [SerializeField]
    [Tooltip ("A float representing the distance a grapple point must be within to be considered viable")]
    private float searchRadius;
    [SerializeField]
    [Tooltip ("The distance needed to successfully grapple")]
    private float stopDistance;
    [SerializeField]
    [Tooltip("The speed of the GameObject heading to the target")]
    private float grappleSpeed;
    [SerializeField]
    [Tooltip("A flag for if the grapple mechanic is being used")]
    private bool grappling;    
    [SerializeField]
    [Tooltip("A flag for if the gameObject is grounded")]
    private bool grounded;



    private void Start()
    {
        AssignComponents();
    }

    private void OnEnable()
    {
        AssignComponents();
    }

    // Update is called once per frame
    private void Update()
    {
        FindTarget();

        if (Input.GetKeyDown (KeyCode.Space))
        {
            if (foundTarget && grounded)
            {
                grappling = true;
            }
            else
            {
                Debug.Log("Unable to find grapple target");
            }
        }

        if (Input.GetKeyUp (KeyCode.Space))
        {
            grappling = false;
            
        }

        if (grappling)
        {
            GrappleToGrapplePoint();
        }
        else
        {
            rigidBody.useGravity = true;
        }

        CreateLine();
    }

    private void AssignComponents()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.Log($"There is no RigidBody component on {this} GameObject!");
        }

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log($"There is no LineRenderer component on {this} GameObject!");
        }
    }

    private void FindTarget()
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

        if (foundTarget)
        {
            target = hookLocs[0].transform;
        }
        else
        {
            target = null;
        }
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

    private void GrappleToGrapplePoint()
    {
        if (Vector3.Distance(transform.position, target.position) > stopDistance)
        {
            rigidBody.useGravity = false;
            transform.position = Vector3.Lerp(transform.position, target.position, grappleSpeed * Time.deltaTime);
        }
        else
        {
            grappling = false;
            Debug.Log($"{Vector3.Distance(transform.position, target.position)}");
        }
    }

    private void CreateLine()
    {
        if (foundTarget && grappling)
        {
            Vector3[] positions = new Vector3[] { target.transform.position, transform.position };

            lineRenderer.enabled = true;

            lineRenderer.SetPositions(positions);
        }
        else
        {
            lineRenderer.enabled = false;
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }       
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }
    }
}
