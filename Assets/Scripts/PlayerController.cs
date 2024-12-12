using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public class PlayerController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionsAsset player_Action_Asset;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movement_Force = 1f;
    [SerializeField]
    private float jump_Force = 5f;
    [SerializeField]
    private float max_Speed = 5f;
    private Vector3 force_Direction = Vector3.zero;
    public float walkThreshold = 0.1f; // Speed threshold for walking
    public float runThreshold = 6f;  // Speed threshold for running 

    [SerializeField]
    private Camera player_Camera;

    public bool Is_Grounded;
    [SerializeField]
    private float dist_To_Ground;

    [SerializeField]
    private bool is_hanging;

    [SerializeField]
    private float ledge_grab_cooldown = 0.3f;
    private float ledge_grab_timer;
    public Animator animator;

    private void Awake()
    {
        ledge_grab_timer = 0;
        rb = GetComponent<Rigidbody>();
        player_Action_Asset = new ThirdPersonActionsAsset();
    }

    private void OnEnable()
    {
        player_Action_Asset.Player.Jump.started += Do_Jump;
        player_Action_Asset.Player.UseItem.started += Use_Item;
        move = player_Action_Asset.Player.Move;
        player_Action_Asset.Player.Enable();
    }

    private void OnDisable()
    {
        player_Action_Asset.Player.Jump.started -= Do_Jump;
        player_Action_Asset.Player.UseItem.started -= Use_Item;
        player_Action_Asset.Player.Disable();
    }

    private void Do_Jump(InputAction.CallbackContext obj)
    {
        if (is_hanging)
        {
            rb.useGravity = true;
            is_hanging = false;
            force_Direction += Vector3.up * jump_Force;
            ledge_grab_timer = 0;
            StartCoroutine(Enable_Movement(0.25f));
            animator.SetBool("Jump", true);
        } else
        if (Check_Grounded())
        {
            force_Direction += Vector3.up * jump_Force;
            animator.SetBool("Jump", true);
        }

        
    }
    
    private void Use_Item(InputAction.CallbackContext obj)
    {
        if (move.enabled)
        {
            Debug.Log("UseItem");
        }
        else
        {
            Debug.Log("Not used");
        }
    }

    IEnumerator Enable_Movement(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        move.Enable();
    }

    private void FixedUpdate()
    {
        LogRigidbodySpeed();

        Ledge_Grab();

        force_Direction += move.ReadValue<Vector2>().x * Get_Camera_Right(player_Camera) * movement_Force;
        force_Direction += move.ReadValue<Vector2>().y * Get_Camera_Forward(player_Camera) * movement_Force;

        rb.AddForce(force_Direction, ForceMode.Impulse);
        force_Direction = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        // Clamp horizontal velocity if it exceeds max speed
        if (horizontalVelocity.sqrMagnitude > max_Speed * max_Speed)
            horizontalVelocity = horizontalVelocity.normalized * max_Speed;

        // Apply the clamped horizontal velocity while preserving vertical velocity
        rb.velocity = horizontalVelocity + Vector3.up * rb.velocity.y;

        // Use the already clamped horizontal velocity for speed calculations
        float horizontalSpeed = horizontalVelocity.magnitude;

        // Check speed thresholds for animations
        if (horizontalSpeed >= runThreshold)
        {
            animator.SetBool("Run", true);  // Character is running
            animator.SetBool("Walk", false); // Character is not walking
        }
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




        Is_Grounded = Check_Grounded();

        if (Is_Grounded && animator.GetBool("Jump"))
        {
            animator.SetBool("Jump", false);
        }
        
        if (!Is_Grounded)
        {
            animator.SetBool("Jump", true);
        }


        if (!is_hanging && animator.GetBool("Climb"))
        {
            animator.SetBool("Climb", false);
        }

        Look_At();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ledge_grab_timer += Time.fixedDeltaTime;

        
    }

    private bool Check_Grounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, dist_To_Ground + 0.1f);
    }

    private Vector3 Get_Camera_Forward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 Get_Camera_Right(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void Look_At()
    {
        if (!move.enabled) return;

        Vector3 direction = rb.velocity;
        direction.y = 0f; // Ignore vertical movement

        if (direction.sqrMagnitude > 0.1f) // Only rotate when moving
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // Prevent jittery rotations
        }
    }


    private void Ledge_Grab()
    {
        if (ledge_grab_timer < ledge_grab_cooldown)
            return;
        if (!Is_Grounded && !is_hanging)
        {
            RaycastHit down_hit;
            Vector3 line_down_start = (transform.position + Vector3.up * 0.5f) + transform.forward;
            Vector3 line_down_end = (transform.position + Vector3.up * 0.1f) + transform.forward;
            Physics.Linecast(line_down_start, line_down_end, out down_hit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(line_down_start, line_down_end, Color.green);

            
            if (down_hit.collider != null)
            {
                RaycastHit forward_hit;
                Vector3 line_forward_start = new Vector3(transform.position.x, down_hit.point.y-0.1f, transform.position.z);
                Vector3 line_forward_end = new Vector3(transform.position.x, down_hit.point.y-0.4f, transform.position.z) + transform.forward * 0.5f;
                Physics.Linecast(line_forward_start, line_forward_end, out forward_hit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(line_forward_start, line_forward_end, Color.red);

                if (forward_hit.collider != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    animator.SetBool("Climb", true);

                    animator.SetBool("Jump", false);
                    
                    is_hanging = true;
                    move.Disable();
                    // hanging animation
                    Vector3 hang_position = new Vector3(forward_hit.point.x, down_hit.point.y, forward_hit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -0.1f;
                    hang_position += offset;
                    transform.position = hang_position;
                }
                
            }
        }


    }

    private void LogRigidbodySpeed()
    {
        if (rb != null)
        {
            // Log Rigidbody velocity
            Vector3 velocity = rb.velocity;
            Debug.Log($"Rigidbody Velocity: X={velocity.x}, Z={velocity.z}, Magnitude={velocity.magnitude}");

            // Calculate horizontal speed
            float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

            Debug.Log("Corrected Horizontal Speed: " + horizontalSpeed);
        }
        else
        {
            Debug.LogError("Rigidbody is null!");
        }
    }




}
