using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaGrappleMechanic : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField]
    private List<Transform> hookLocs = new List<Transform>();

    [Space(10)]

    [Header("Properties")]
    [SerializeField]
    private bool foundTarget;
    [SerializeField]
    private LayerMask grapplePoint;
    [SerializeField]
    private float searchRadius = 5000;



    // Update is called once per frame
    void Update()
    {
        FindTarget();
    }

    public void FindTarget()
    {
        hookLocs.Clear();

        Collider[] hooksCol = Physics.OverlapSphere(transform.position, searchRadius, grapplePoint);

        for (int i = 0; i < hooksCol.Length; i++)
        {
            hookLocs.Add(hooksCol[i].transform);
        }

        Debug.Log(hooksCol);

        if (hookLocs.Count > 0)
        {
            foundTarget = true;
        }
        else
        {
            foundTarget = false;
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
}
