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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                stepTimer = stepInterval; // Reset timer
            }
        }
    }

    private void PlayFootstep(float speed)
    {
        // Adjust step interval based on walking or running
        stepInterval = speed > runSpeedThreshold ? 0.3f : 0.5f;

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

    private void OnTriggerEnter(Collider other)
    {
        /* Detect surface type using tags
        if (other.CompareTag("Ground"))
        {
            currentSurface = "Default";
        }
        */
    }

    private bool IsGrounded()
    {
        // Check if the player is on the ground layer using Raycast
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

    }
}
