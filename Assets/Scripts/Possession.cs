using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    public float raycast_Range = 100f;  // Maximum distance for raycasting
    public LayerMask target_Layer;     // Layer mask to filter what the raycast can hit
    private GameObject controlled_Object;  // The currently controlled object
    private PlayerController character_Controller;
   
    Cinemachine.CinemachineFreeLook free_Look;

    void Start()
    {
        // Assuming the playable character is the one the script is initially attached to
        controlled_Object = this.gameObject;
        character_Controller = controlled_Object.GetComponent<PlayerController>();
        
    }

    void Update()
    {
        // Handle input for raycast and control transfer
        if (Input.GetKeyDown(KeyCode.K)) // On left-click, try to transfer control
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycast_Range, target_Layer))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object is a valid new control target (has a PlayerController, for example)
                if (hitObject != null && hitObject.GetComponent<PlayerController>() != null)
                {
                    TransferCharacterControl(hitObject);
                }
            }
        }

        // Allow movement for the controlled object (if it's a character controller)
        if (controlled_Object != null && character_Controller != null)
        {
           character_Controller.enabled = true;
        }
    }

    void TransferCharacterControl(GameObject newObject)
    {
        if (newObject != controlled_Object)
        {
            Debug.Log("Transferring control to: " + newObject.name);

            // Disable the character controller of the current object
            if (character_Controller != null)
            {
                character_Controller.enabled = false;
            }

            // Set the new controlled object and update its character controller
            controlled_Object = newObject;
            character_Controller = controlled_Object.GetComponent<PlayerController>();
            free_Look = Camera.main.GetComponent<CinemachineFreeLook>();
            if (free_Look != null)
            {
                free_Look.LookAt=controlled_Object.transform;
                free_Look.Follow=controlled_Object.transform;
            }
            // Enable the character controller for the new object
            if (character_Controller != null)
            {
                character_Controller.enabled = true;
            }
        }
    }
}

    

