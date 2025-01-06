using Cinemachine;
using UnityEngine;

public class Possession : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float raycastRange = 100f;  // Maximum distance for raycasting
    public LayerMask targetLayer;     // Layer mask to filter what the raycast can hit

    [Header("Cinemachine Settings")]
    public CinemachineFreeLook freeLookCamera; // Reference to the Cinemachine FreeLook Camera

    private GameObject controlledObject;       // The currently controlled object
    private PlayerController characterController;

    void Start()
    {
        // Initialize with the object this script is attached to
        controlledObject = gameObject;
        characterController = controlledObject.GetComponent<PlayerController>();

        if (freeLookCamera == null)
        {
            freeLookCamera = Camera.main.GetComponent<CinemachineFreeLook>();
        }

        UpdateCameraTarget(controlledObject);
    }

    void Update()
    {
        // Check for possession input
        if (Input.GetKeyDown(KeyCode.K))
        {
            TryPossess();
        }
    }

    void TryPossess()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, targetLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Highlight the object
            var highlight = hitObject.GetComponent<PossessableHighlight>();
            if (highlight != null)
            {
                highlight.EnableGlow();
            }

            // Check if the hit object is a valid target
            if (hitObject != null && hitObject.GetComponent<PlayerController>() != null)
            {
                TransferControl(hitObject);
            }
        }
        else
        {
            // Disable glow on all objects if not hovering
            foreach (var highlight in FindObjectsOfType<PossessableHighlight>())
            {
                highlight.DisableGlow();
            }
        }
    }


    private void TransferControl(GameObject newObject)
    {
        if (newObject == controlledObject) return; // Avoid re-possessing the same object

        Debug.Log($"Transferring control to: {newObject.name}");

        // Disable the current controlled object's controller
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Update controlled object and enable its controller
        controlledObject = newObject;
        characterController = controlledObject.GetComponent<PlayerController>();
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        // Update Cinemachine to follow the new controlled object
        UpdateCameraTarget(controlledObject);
    }

    private void UpdateCameraTarget(GameObject target)
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = target.transform;
            freeLookCamera.LookAt = target.transform;
        }
    }
}
