using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoeMovementScript : MonoBehaviour
{
    Animator animator;
    public Rigidbody parentRb;
    public float walkThreshold = 0.1f; // Speed threshold for walking
    public float runThreshold = 6f;  // Speed threshold for running 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (transform.parent != null)
        {
            parentRb = transform.parent.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementState();
        LogRigidbodySpeed();
    }

    private void UpdateMovementState()
    {
        if (parentRb != null)
        {
            // Calculate horizontal speed from the parent's Rigidbody
            float horizontalSpeed = new Vector3(parentRb.velocity.x, 0, parentRb.velocity.z).magnitude;

            // Check if the speed is above the run threshold for running
            if (horizontalSpeed >= runThreshold)
            {
                animator.SetBool("Run", true);  // Character is running
                animator.SetBool("Walk", false); // Character is not walking
            }
            // Check if the speed is above the walk threshold for walking
            else if (horizontalSpeed >= walkThreshold)
            {
                animator.SetBool("Run", false); // Character is not running
                animator.SetBool("Walk", true); // Character is walking
            }
            else
            {
                animator.SetBool("Run", false);  // Character is idle
                animator.SetBool("Walk", false); // Character is idle
            }
        }
    }

    private void LogRigidbodySpeed()
    {
        if (parentRb != null)
        {
            // Calculate the magnitude of the Rigidbody's velocity
            Vector3 velocity = parentRb.velocity;
            float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

            // Log the full velocity to track what's happening
            Debug.Log("Horizontal Speed: " + horizontalSpeed);
        }
    }
}
