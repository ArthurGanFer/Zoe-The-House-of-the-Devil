using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool mainCharacter;
    [SerializeField]
    public bool isActiveCharacter;

    // Input fields
    public ThirdPersonActionsAsset player_Action_Asset;
    private InputAction move;

    // Movement fields
    public Rigidbody rb;
    [SerializeField]
    private float movement_Force = 1f;
    [SerializeField]
    private float jump_Force = 5f;
    [SerializeField]
    private float max_Speed = 5f;
    private Vector3 force_Direction = Vector3.zero;
    public float walkThreshold = 0.1f; // Speed threshold for walking
    public float runThreshold = 6f;  // Speed threshold for running 

    public float crouchMovementForce = 0.5f;
    public float crouchMaxSpeed = 2f;

    [SerializeField]
    private Camera player_Camera;

    public bool Is_Grounded;
    [SerializeField]
    private float dist_To_Ground;

    [SerializeField]
    private bool is_hanging;

    [SerializeField]
    public bool is_crouching = false;

    [SerializeField]
    private float ledge_grab_cooldown = 0.3f;
    private float ledge_grab_timer;
    public Animator animator;

    private WaitForSeconds possession_timer = new WaitForSeconds(10f);

    public Vector3 resetPos;
    public Quaternion resetRotation;

    [SerializeField] private GameObject match_obj;
    [SerializeField]
    private int matches_available = 0;
    [SerializeField]
    private bool is_using_match = false;
    private WaitForSeconds match_timer = new WaitForSeconds(18f);

    private void Awake()
    {
        player_Action_Asset = new ThirdPersonActionsAsset();
        if (mainCharacter)
        {
            isActiveCharacter = true;
        }
        else
        {
            isActiveCharacter = false;
        }
    }

    protected virtual void AssignComponents()
    {
        resetPos = this.transform.position;
        resetRotation = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log($"There is no RigidBody component on {this} GameObject!");
        }
        if (player_Action_Asset == null)
        {
            Debug.Log($"There is no ThirdPersonActionsAsset component on {gameObject} GameObject!");
        }
        else
        {
            move = player_Action_Asset.Player.Move;
            Debug.Log("move is set to " + gameObject.name);
        }
    }

    protected virtual void UnassignComponents()
    {
        rb = null;
        player_Action_Asset = null;
    }

    private void OnEnable()
    {
        AssignComponents();
        if (mainCharacter)
        {
            EnableControllers();
        }
    }

    public void EnableControllers()
    {
        player_Action_Asset.Player.Jump.started += Do_Jump;
        player_Action_Asset.Player.UseItem.started += Use_Item;
        player_Action_Asset.Player.Crouch.started += Do_Crouch;
        player_Action_Asset.Player.Possess.started += Do_Possess;
        player_Action_Asset.Player.UseItem.performed += Use_Item;
        player_Action_Asset.Player.UseItem.canceled += Use_Item;
        move = player_Action_Asset.Player.Move;
        move.Enable();
        player_Action_Asset.Player.Enable();
    }

    private void OnDisable()
    {
        DisableControllers();
        UnassignComponents();
    }

    public void DisableControllers()
    {
        player_Action_Asset.Player.Jump.started -= Do_Jump;
        player_Action_Asset.Player.UseItem.started -= Use_Item;
        player_Action_Asset.Player.Crouch.started -= Do_Crouch;
        player_Action_Asset.Player.Possess.started -= Do_Possess;
        player_Action_Asset.Player.UseItem.performed -= Use_Item;
        player_Action_Asset.Player.UseItem.canceled -= Use_Item;
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
        }
        else if (Check_Grounded())
        {
            force_Direction += Vector3.up * jump_Force;
            animator.SetBool("Jump", true);
        }
    }

    protected virtual void Use_Item(InputAction.CallbackContext obj)
    {
        if (move.enabled && matches_available > 0 && !is_using_match) 
        {
            Debug.Log("Using match");
            StartCoroutine(LightMatch());
            animator.SetBool("Attack", true);
            StartCoroutine(ResetAttackAnimation());
            animator.SetTrigger("Match"); 
        }
        else
        {
            Debug.Log("Cannot use match: No matches available, movement disabled, or already using a match");
        }
    }

    public void Do_Crouch(InputAction.CallbackContext obj)
    {
        if (!is_crouching && move.enabled)
        {
            is_crouching = true;
            Debug.Log("Crouching");
            animator.SetBool("Crouch", true);
        }
        else
        {
            is_crouching = false;
            Debug.Log("Standing");
            animator.SetBool("Crouch", false);
        }
    }

    protected virtual void Do_Possess(InputAction.CallbackContext obj)
    {
        if (Check_Avatar_Doll() != null)
        {
            StartCoroutine(PossessionCycle(Check_Avatar_Doll()));
        }
    }

    IEnumerator Enable_Movement(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        move.Enable();
    }

    protected virtual void FixedUpdate()
    {
        if (move.enabled)
        {
            float current_Movement_Force = is_crouching ? crouchMovementForce : movement_Force;
            float current_Max_Speed = is_crouching ? crouchMaxSpeed : max_Speed;

            force_Direction += move.ReadValue<Vector2>().x * Get_Camera_Right(player_Camera) * current_Movement_Force;
            force_Direction += move.ReadValue<Vector2>().y * Get_Camera_Forward(player_Camera) * current_Movement_Force;

            rb.AddForce(force_Direction, ForceMode.Impulse);
            force_Direction = Vector3.zero;

            if (rb.velocity.y < 0f)
                rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;

            if (horizontalVelocity.sqrMagnitude > current_Max_Speed * current_Max_Speed)
                horizontalVelocity = horizontalVelocity.normalized * current_Max_Speed;

            rb.velocity = horizontalVelocity + Vector3.up * rb.velocity.y;

            float horizontalSpeed = horizontalVelocity.magnitude;

            if (horizontalSpeed >= runThreshold)
            {
                animator.SetBool("Run", true);
                animator.SetBool("Walk", false);
            }
            else if (horizontalSpeed >= walkThreshold)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
            }
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
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private PlayerController Check_Avatar_Doll()
    {
        Collider[] dolls = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Player"));
        foreach (var doll in dolls)
        {
            if (doll.GetComponent<PlumberController>() != null)
            {
                if (doll.GetComponent<PlumberController>().mainCharacter == false)
                {
                    return doll.GetComponent<PlumberController>();
                }
            }
        }
        return null;
    }

    IEnumerator PossessionCycle(PlayerController targetAvatar)
    {
        ResetAnimator(this.animator);
        CinemachineFreeLook cinemachine = player_Camera.GetComponent<CinemachineFreeLook>();
        this.DisableControllers();
        targetAvatar.EnableControllers();
        cinemachine.Follow = targetAvatar.transform;
        cinemachine.LookAt = targetAvatar.transform;
        this.isActiveCharacter = false;
        targetAvatar.isActiveCharacter = true;
        yield return possession_timer;
        ResetAnimator(targetAvatar.animator);
        this.isActiveCharacter = true;
        targetAvatar.isActiveCharacter = false;
        targetAvatar.DisableControllers();
        targetAvatar.ResetAvatarPosition();
        this.EnableControllers();
        cinemachine.Follow = this.transform;
        cinemachine.LookAt = this.transform;
    }

    private void ResetAvatarPosition()
    {
        transform.position = resetPos;
        transform.rotation = resetRotation;
    }

    IEnumerator LightMatch()
    {
        if (matches_available > 0 && !is_using_match)
        {
            matches_available -= 1;
            is_using_match = true;
            match_obj.SetActive(true);

            yield return match_timer;

            match_obj.SetActive(false);
            is_using_match = false;
            animator.SetTrigger("Match");
        }
    }

    public void AddMatch()
    {
        matches_available += 1;
    }

    public void TeleportToPosition(Transform target)
    {
        this.transform.position = target.position;
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("Attack", false);
    }

    private void ResetAnimator(Animator animator)
    {
        animator.gameObject.SetActive(false);
        animator.gameObject.SetActive(true);
    }
}