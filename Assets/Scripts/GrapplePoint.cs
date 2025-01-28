using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Camera mainCamera;

    [Header("Properties")]
    [SerializeField]
    private bool isActive;


    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible())
        {
            isActive = true;
            this.gameObject.layer = LayerMask.NameToLayer("GrapplePoint");
            Debug.Log($"{this} is seen by camera");
        }
        else
        {
            isActive = false;
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void AssignComponents()
    {
        mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null)
        {
            Debug.Log("There is no GameObject with a Camera Component in the scene!");
        }
    }

    private bool isVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return planes.All(plane => plane.GetDistanceToPoint(this.transform.position) >= 0);
    }
}
