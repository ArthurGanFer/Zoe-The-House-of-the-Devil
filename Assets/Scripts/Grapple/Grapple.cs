using System.Collections;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private float travel_Speed = 10f;       // Speed of pulling
    [SerializeField] private float travel_Distance = 20f;    // Max distance for the grapple
    [SerializeField] private GameObject hook_Prefab;         // Hook prefab
    [SerializeField] private Transform hook_Transform;       // Origin point for the hook

    private Rigidbody rb;                                    // Rigidbody for the player
    private HookShot hook_Shot;                              // Reference to the active hook
    private bool is_travelling = false;                      // Is the player being pulled
    private bool is_hooked = false;                          // Is the hook active
    private Vector3 pull_direction;                         // Direction of pull

    private Vector3 startpos;                                // Starting position

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startpos = transform.position;
    }

    private void Update()
    {
        // Input to activate or deactivate the hook
        if (Input.GetKeyDown(KeyCode.Q))
        {
            is_hooked = true;
            Create_Hook();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            is_hooked = false;
            Stop_Pull();
            Destroy_Hook();
        }

        // Reset position
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.position = startpos;
            Stop_Pull();
        }

        // Pulling logic
        if (is_travelling && hook_Shot != null)
        {
            float distance = Vector3.Distance(transform.position, hook_Shot.transform.position);
            if (distance < 0.5f || distance > travel_Distance) // Stop if too close or out of range
            {
                Stop_Pull();
                Destroy_Hook();
            }
        }
    }

    public IEnumerator Start_Pull()
    {
        is_travelling = true;
        while (is_travelling && hook_Shot != null)
        {
            // Calculate pull direction and move the player
            pull_direction = (hook_Shot.transform.position - transform.position).normalized;
            rb.AddForce(pull_direction,ForceMode.VelocityChange);

            yield return null; // Wait until the next frame
        }
    }

    private void Stop_Pull()
    {
        is_travelling = false;
        rb.velocity = Vector3.zero; // Stop all movement
    }

    private void Create_Hook()
    {
        if (hook_Shot == null && is_hooked)
        {
            hook_Shot = Instantiate(hook_Prefab, hook_Transform.position, hook_Transform.rotation).GetComponent<HookShot>();
            hook_Shot.Initialize(this, hook_Transform);
        }
    }

  public void Destroy_Hook()
    {
        if (hook_Shot != null)
        {
            Destroy(hook_Shot.gameObject);
            hook_Shot = null;
        }
        is_hooked = false;
        Stop_Pull();
    }
}

