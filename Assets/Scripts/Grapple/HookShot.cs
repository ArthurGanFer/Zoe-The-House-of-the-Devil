using UnityEngine;

public class HookShot : MonoBehaviour
{
    [SerializeField] private float hook_Speed = 50f;         // Speed of the hook
    [SerializeField] private float max_Hook_Distance = 30f;  // Maximum distance before the hook is destroyed

    private Rigidbody rb;
    private Grapple grapple;
    private LineRenderer line_Renderer;

    public void Initialize(Grapple grapple, Transform origin_transform)
    {
        // Align the hook to the origin transform
        transform.position = origin_transform.position + Vector3.up; // Offset 1 unit above
        transform.rotation = origin_transform.rotation;

        this.grapple = grapple;

        // Set up Rigidbody and LineRenderer
        rb = GetComponent<Rigidbody>();
        line_Renderer = GetComponent<LineRenderer>();

        // Apply velocity for parabolic motion
        Vector3 forwardVelocity = transform.forward * hook_Speed;
        Vector3 upwardVelocity = Vector3.up * Mathf.Sqrt(2 * Physics.gravity.magnitude * 2f); // Peak height ~2 units
        rb.velocity = forwardVelocity + upwardVelocity;
    }

    private void Update()
    {
        // Update the line renderer for a visual tether
        if (grapple != null)
        {
            line_Renderer.SetPosition(0, transform.position);
            line_Renderer.SetPosition(1, grapple.transform.position);
        }

        // Destroy the hook if it exceeds max distance
        if (Vector3.Distance(transform.position, grapple.transform.position) > max_Hook_Distance)
        {
            grapple.Destroy_Hook();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Hook_Shot") & 1 << other.gameObject.layer) > 0)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            grapple.StartCoroutine(grapple.Start_Pull()); // Start pulling
        }
    }
}
