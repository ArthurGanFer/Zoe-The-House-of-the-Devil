using System.Linq;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField]
    [Tooltip ("A reference to our Camera Component")]
    private Camera mainCamera;
    [SerializeField]
    [Tooltip("A reference to our GrappleMechanic Component")]
    private FisherController grappleMechanic;

    [Space (10)]
    [Header ("Properties")]
    [SerializeField]
    [Tooltip ("A flag for if this GameObject is a viable grapple point")]
    private bool isActive;



    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (grappleMechanic != null)
        {
            if (isVisible() || (grappleMechanic.grappling == true && this.gameObject == grappleMechanic.target))
            {
                isActive = true;
                
                //Debug.Log($"{this} is seen by camera");
            }
            else
            {
                isActive = false;
            }

            if (isActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("GrapplePoint");
            }
            else
            {
                this.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            CheckForGrappleMechanic();
        }

    }

    private void AssignComponents()
    {
        mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null)
        {
            Debug.Log("There is no GameObject with a Camera Component in the scene!");
        }

        grappleMechanic = FindObjectOfType<FisherController>();
        if (grappleMechanic == null)
        {
            Debug.Log("There is no GameObject with a FisherController Component in the scene!");
        }
    }

    private bool isVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return planes.All(plane => plane.GetDistanceToPoint(this.transform.position) >= 0);
    }

    private void CheckForGrappleMechanic()
    {
        if (grappleMechanic == null)
        {
            FisherController[] availableGrappleMechanics = FindObjectsOfType<FisherController>();

            foreach(FisherController grapple in availableGrappleMechanics)
            {
                if (grapple == grappleMechanic.GetComponent<PlayerController>().isActiveCharacter)
                {
                    grappleMechanic = FindObjectOfType<FisherController>();
                    break;
                }
            }
        }
        else
        {
            return;
        }
    }
}
