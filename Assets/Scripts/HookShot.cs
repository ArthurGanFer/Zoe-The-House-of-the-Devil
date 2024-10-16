using UnityEngine;

public class HookShot : MonoBehaviour
{
    // Serialized fields for adjusting the hook behavior
    [SerializeField] private Transform hook_Origin; // The origin point from where the hook is shot
    [SerializeField] private float hook_Speed = 50f; // Speed of the hook
    [SerializeField] private float max_Hook_Distance; // Maximum distance the hook can travel

    private Rigidbody rb; // Rigidbody to apply physics-based movement
    private Grapple grapple; // Reference to the Grapple script for interaction
    private LineRenderer line_Renderer; // LineRenderer to visually show the hook line
    public Camera Main_Camera; // Camera reference, if needed

    public Vector3 Target_Position; // Stores the position of the object the hook collides with

    // Initialize the hook's settings when instantiated
    public void Initialize(Grapple grapple, Transform origin_transform)
    {
        // Align the hook's forward direction with the origin's forward direction
        transform.forward = origin_transform.forward;

        this.grapple = grapple; // Set the grapple reference
        rb = GetComponent<Rigidbody>(); // Cache the Rigidbody component
        line_Renderer = GetComponent<LineRenderer>(); // Cache the LineRenderer component

        // Launch the hook in the forward direction with a set speed
        rb.AddForce(transform.forward * hook_Speed, ForceMode.VelocityChange);
    }

    // Called when the hook collides with another object
    private void OnTriggerEnter(Collider other)
    {
        // Print the name of the object the hook hits
        print(other.name);

        // Check if the collided object's layer matches the "Hook_Shot" layer
        if ((LayerMask.GetMask("Hook_Shot") & 1 << other.gameObject.layer) > 0)
        {
            Target_Position = other.transform.position; // Store the position of the object hit
            rb.useGravity = false; // Disable gravity for the hook
            rb.isKinematic = true; // Make the hook kinematic to stop movement

            // Start pulling the player towards the hook using the grapple's method
            StartCoroutine(grapple.Start_Pull());
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // Continuously update the line renderer's positions to visually represent the rope/line
        Vector3[] positions = new Vector3[] { transform.position, grapple.transform.position };
        line_Renderer.SetPositions(positions);
    }
}