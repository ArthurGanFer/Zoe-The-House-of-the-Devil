using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> defaultFootsteps; // Generic footsteps
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer;
    private string currentSurface = "Default";

    private Rigidbody rb;
    [SerializeField] private float walkSpeedThreshold = 0.1f; // Speed for walking
    [SerializeField] private float runSpeedThreshold = 6f;    // Speed for running

    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float minVolume = 0.6f;
    [SerializeField] private float maxVolume = 1.0f;

    [SerializeField] private LayerMask groundLayer;  // LayerMask for ground
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance for ground check

    private PlayerController playerController; // Reference to PlayerController

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>(); // Get the PlayerController component
    }

    private void Update()
    {
        float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        if (IsGrounded() && horizontalSpeed > walkSpeedThreshold)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep(horizontalSpeed);
                stepTimer = GetStepInterval(horizontalSpeed); // Reset timer based on crouch state
            }
        }
    }

    private void PlayFootstep(float speed)
    {
        AudioClip clip = GetFootstepClip();

        // Set random pitch and volume
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);

        // Play the footstep sound
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetFootstepClip()
    {
        List<AudioClip> footstepSounds = currentSurface switch
        {
            _ => defaultFootsteps,
        };

        return footstepSounds[Random.Range(0, footstepSounds.Count)];
    }

    private float GetStepInterval(float speed)
    {
        // Adjust step interval based on crouch state and speed
        if (playerController != null && playerController.is_crouching)
        {
            // Slower footsteps when crouching
            return speed > runSpeedThreshold ? 0.6f : 0.9f; // Longer interval for crouching
        }
        else
        {
            // Normal footsteps when not crouching
            return speed > runSpeedThreshold ? 0.3f : 0.5f;
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is on the ground layer using Raycast
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}