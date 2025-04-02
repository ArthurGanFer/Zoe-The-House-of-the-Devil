using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FisherController : PlayerController
{
    [Header ("Components")]
    [SerializeField]
    [Tooltip ("A list of Transforms representing possible grapple points")]
    private List<Transform> hookLocs = new List<Transform>();
    [Tooltip("Our target grapple point")]
    public Transform target;
    [SerializeField]
    [Tooltip("The transform where our line will start")]
    public Transform shootTransform;
    [SerializeField]
    [Tooltip ("A reference to our LineRenderer Component")]
    private LineRenderer lineRenderer;
    
    [Space(10)]
    
    [Header ("Properties")]
    
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
    [Tooltip ("The speed of the GameObject heading to the target")]
    private float grappleSpeed;
    [Tooltip ("A flag for if the grapple mechanic is being used")]
    public bool grappling;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        FindTarget();

        if (grappling)
        {
            GrappleToGrapplePoint();
            //animator.SetBool("Attack", true);

        }
        else
        {
            rb.useGravity = true;
            //animator.SetBool("Attack", false);

        }

        CreateLine();
        
    }

    protected override void AssignComponents()
    {
        base.AssignComponents();
        
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log($"There is no LineRenderer component on {this} GameObject!");
        }

        shootTransform = GetComponentInChildren<Transform>();
        if (shootTransform == null)
        {
            Debug.Log($"There is no Transform component on {this} GameObject child!");
        }
    }
    
    protected override void UnassignComponents()
    {
        base.UnassignComponents();
        lineRenderer = null;
        shootTransform = null;
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
            target = hookLocs[0].transform;
        }
        else
        {
            target = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (target != null)
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
        if (Vector3.Distance(this.transform.position, this.target.position) > this.stopDistance)
        {
            this.rb.useGravity = false;
            this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, this.grappleSpeed * Time.deltaTime);
        }
        else
        {
            grappling = false;
            //Debug.Log($"{Vector3.Distance(transform.position, target.position)}");
        }
    }
    
    private void CreateLine()
    {
        if (target != null && grappling)
        {
            Vector3[] positions = new Vector3[] { shootTransform.transform.position, transform.position };

            lineRenderer.enabled = true;

            lineRenderer.SetPositions(positions);
        }
        else
        {
            lineRenderer.enabled = false;
            return;
        }
    }
    
    protected override void Use_Item(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            if (target != null && Is_Grounded)
            {
                grappling = true;
            }
            else
            {
                Debug.Log("Unable to find grapple target");
            }
        }

        if (obj.canceled)
        {
            grappling = false;
        }
    }

    protected override void Do_Possess(InputAction.CallbackContext obj)
    {
        Debug.Log("Not main character");
    }
}
