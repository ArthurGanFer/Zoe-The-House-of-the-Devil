using UnityEngine;

public class hookMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private grappleMechanic grappleMechanic;
    [SerializeField]
    private Rigidbody hookRB;
    [SerializeField]
    private LineRenderer hookLR;

    [Space(10)]

    [Header("Properties")]
    [SerializeField]
    private LayerMask grapplePoint;
    [SerializeField]
    private float hookForce = 25f;
    [SerializeField]
    public Transform[] hookLocs;
    [SerializeField]
    private bool foundTarget;

    public void InitializeGrapple(grappleMechanic grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grappleMechanic = grapple;
        AssignComponents();
        hookRB.AddForce(transform.forward * hookForce, ForceMode.Impulse);
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
        
        Vector3[] positions = new Vector3[] {transform.position, grappleMechanic.transform.position};

        hookLR.SetPositions(positions);
        
       
        //FindTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((grapplePoint & 1 << other.gameObject.layer) > 0)
        {
            hookRB.useGravity = false;
            hookRB.isKinematic = true;

            grappleMechanic.StartPull();
        }
    }

    public void FindTarget()
    {
        Collider[] hooksCol = Physics.OverlapSphere(transform.position, 5000, grapplePoint);
        for (int i = 0; i < hooksCol.Length; i++)
        {

            hookLocs[i] = hooksCol[i].transform;
        }
        Debug.Log(hooksCol);

        if (hookLocs.Length > 0)
        {
            foundTarget = true;
        }

    }
}
