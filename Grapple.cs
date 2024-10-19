using System.Collections;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    // Public and private fields for controlling the grapple behavior
    [SerializeField] float travel_Speed;       // Speed at which the player travels toward the hook
    [SerializeField] float travel_Distance;    // Maximum distance the player can travel using the grapple
    [SerializeField] GameObject hook_Prefab;   // Prefab of the hook shot to instantiate
    [SerializeField] Transform hook_Transform; // The point where the hook will spawn from

    private ThirdPersonActionsAsset test_Controls;       // Reference to a control script (For input management)
    private Vector3 startpos;                  // Stores the initial position of the player

    private HookShot hook_Shot;                // Instance of the hook shot
    private bool is_travelling = false;        // Tracks if the player is currently being pulled by the hook
    private Rigidbody rb;                      // Rigidbody for applying physics-based movement
    public bool Is_Hooked = false;             // Tracks if the hook is currently active

    // Initialize the test controls on object creation
    private void Awake()
    {
        test_Controls = new ThirdPersonActionsAsset();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    // Cache the Rigidbody component
        is_travelling = false;             // Player is not travelling at the start
        startpos = this.transform.position; // Store the starting position
    }

    // Coroutine to apply pulling force towards the hook
    public IEnumerator Start_Pull()
    {
        is_travelling = true;
        Debug.Log("Do Pulling");

        Vector3 direction = (hook_Shot.transform.position - transform.position).normalized;
        rb.AddForce(direction * travel_Speed, ForceMode.VelocityChange); // Apply force towards the hook

        yield return new WaitForSeconds(8f); // Wait for 8 seconds before stopping the pull
    }

    // Update is called once per frame
    void Update()
    { /// Currently operating on hard coded options
        // Toggle hook state based on key inputs
        if (Input.GetKey(KeyCode.Q))
        {
            Is_Hooked = true; // Activate hook
        }
        else if (Input.GetKey(KeyCode.R))
        {
            Is_Hooked = false; // Deactivate hook
        }

        // Reset player position to starting position on key press
        if (Input.GetKey(KeyCode.G))
        {
            transform.position = startpos;
        }

        // If no hook exists and hook is activated, instantiate a new hook
        if (hook_Shot == null && Is_Hooked == true)
        {
            Debug.Log("Hook Activated");
            is_travelling = false;

            // Instantiate hook at the hook transform's position and initialize it
            hook_Shot = Instantiate(hook_Prefab, hook_Transform.position, Quaternion.identity).GetComponent<HookShot>();
            hook_Shot.Initialize(this, hook_Transform);

            StopAllCoroutines(); // Stop any existing coroutines to avoid multiple pulls
            StartCoroutine(Destroy_Hook_Life_Span()); // Start hook lifespan timer
        }
        // If hook exists but hook is deactivated, destroy the hook
        else if (hook_Shot != null && Is_Hooked != true)
        {
            Debug.Log("Hook Deactivated");
            Destroy_Hook();
        }

        // Stop execution if the player is not travelling or if the hook doesn't exist
        if (!is_travelling || hook_Shot == null) return;

        // Destroy hook if player exceeds travel distance
        if (Vector3.Distance(transform.position, hook_Transform.position) >= travel_Distance)
        {
            Debug.Log("Exceeded Travel Distance");
            Destroy_Hook();
        }
        // If hook is active, continue pulling the player
        else if (hook_Shot != null && Is_Hooked == true)
        {
            StartCoroutine(Start_Pull());
        }
    }

    // Coroutine to destroy the hook after a certain lifespan (8 seconds)
    private IEnumerator Destroy_Hook_Life_Span()
    {
        yield return new WaitForSeconds(8f);
        Debug.Log("Hook Lifespan Ended");
        Destroy_Hook();
    }

    // Destroys the hook object and resets related variables
    public void Destroy_Hook()
    {
        if (hook_Shot == null) return; // Exit if no hook exists
        is_travelling = false;
        Is_Hooked = false;

        // Destroy the hook object and clear reference
        Destroy(hook_Shot.gameObject);
        hook_Shot = null;
    }
}
