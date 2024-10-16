using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    public float raycastRange = 100f;  // Maximum distance for raycasting
    public LayerMask targetLayer;     // Layer mask to filter what the raycast can hit
    private GameObject controlledObject;  // The currently controlled object
    private CharacterController characterController;

    void Start()
    {
        // Assuming the playable character is the one the script is initially attached to
        controlledObject = this.gameObject;
        characterController = controlledObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Handle input for raycast and control transfer
        if (Input.GetKeyDown(KeyCode.K)) // On left-click, try to transfer control
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastRange, targetLayer))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object is a valid new control target (has a CharacterController, for example)
                if (hitObject != null && hitObject.GetComponent<CharacterController>() != null)
                {
                    TransferCharacterControl(hitObject);
                }
            }
        }

        // Allow movement for the controlled object (if it's a character controller)
        if (controlledObject != null && characterController != null)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            characterController.Move(movement * Time.deltaTime * 5f); // Move the controlled object
        }
    }

    void TransferCharacterControl(GameObject newObject)
    {
        if (newObject != controlledObject)
        {
            Debug.Log("Transferring control to: " + newObject.name);

            // Disable the character controller of the current object
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            // Set the new controlled object and update its character controller
            controlledObject = newObject;
            characterController = controlledObject.GetComponent<CharacterController>();

            // Enable the character controller for the new object
            if (characterController != null)
            {
                characterController.enabled = true;
            }
        }
    }
}

    

