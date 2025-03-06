using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GrappleMechanic : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField]
    [Tooltip ("A reference to our ThirdPersonActionsAsset component")]
    private ThirdPersonActionsAsset thirdPersonActionAsset;
    [SerializeField]
    [Tooltip ("A list of Transforms representing possible grapple points")]
    private List<Transform> hookLocs = new List<Transform>();
    [Tooltip("Our target grapple point")]
    public Transform target;
    [SerializeField]
    [Tooltip("The transform where our line will start")]
    public Transform shootTransform;
    [SerializeField]
    [Tooltip ("A reference to our RigidBody Component")]
    private Rigidbody rigidBody;
    [SerializeField]
    [Tooltip ("A reference to our LineRenderer Component")]
    private LineRenderer lineRenderer;
    [SerializeField]
    [Tooltip("A reference to our PlayerController component")]
    private PlayerController playerController;

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
    [SerializeField]
    [Tooltip ("A flag for if the gameObject is grounded")]
    private bool grounded;



    private void Start()
    {
        AssignComponents();
    }

    private void OnEnable()
    {
        AssignComponents();
    }

    private void OnDisable()
    {
        UnassignComponents();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target != null && grounded)
            {
                grappling = true;
            }
            else
            {
                Debug.Log("Unable to find grapple target");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            grappling = false;
        }

        FindTarget();

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

        shootTransform = GetComponentInChildren<Transform>();
        if (shootTransform == null)
        {
            Debug.Log($"There is no Transform component on {this} GameObject child!");
        }

        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log($"There is no GameObject of type PlayerController in {SceneManager.GetActiveScene()}!");
        }
        else
        {
            thirdPersonActionAsset = playerController.GetComponent<ThirdPersonActionsAsset>();
            if (thirdPersonActionAsset == null)
            {
                Debug.Log($"There is no ThirdPersonActionsAsset component on {playerController.gameObject} GameObject!");
            }
            else
            {
                thirdPersonActionAsset.Player.UseItem.started += UseGrapple;
            }
        }
    }

    private void UnassignComponents()
    {
        rigidBody = null;
        lineRenderer = null;
        shootTransform = null;
        playerController = null;
        thirdPersonActionAsset = null;

        thirdPersonActionAsset.Player.UseItem.started -= UseGrapple;
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
        if (this.target != null)
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
            this.rigidBody.useGravity = false;
            this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, this.grappleSpeed * Time.deltaTime);
        }
        else
        {
            this.grappling = false;
            //Debug.Log($"{Vector3.Distance(transform.position, target.position)}");
        }
    }

    private void CreateLine()
    {
        if (this.target != null && this.grappling)
        {
            Vector3[] positions = new Vector3[] {this.shootTransform.transform.position, this.transform.position };

            this.lineRenderer.enabled = true;

            this.lineRenderer.SetPositions(positions);
        }
        else
        {
            this.lineRenderer.enabled = false;
            return;
        }
    }

    private void UseGrapple(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            if (target != null && grounded)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer($"Ground"))
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
