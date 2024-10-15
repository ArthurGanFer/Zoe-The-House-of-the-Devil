using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Camera player_Camera;

    public bool Is_Grounded;
    [SerializeField]
    private float dist_To_Ground;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player_Action_Asset = new ThirdPersonActionsAsset();
    }

    private void OnEnable()
    {
        player_Action_Asset.Player.Jump.started += Do_Jump;
        move = player_Action_Asset.Player.Move;
        player_Action_Asset.Player.Enable();
    }

    private void OnDisable()
    {
        player_Action_Asset.Player.Jump.started -= Do_Jump;
        player_Action_Asset.Player.Disable();
    }

    private void Do_Jump(InputAction.CallbackContext obj)
    {
        if (Check_Grounded())
        {
            force_Direction += Vector3.up * jump_Force;
        }
    }

    private void FixedUpdate()
    {
        force_Direction += move.ReadValue<Vector2>().x * Get_Camera_Right(player_Camera) * movement_Force;
        force_Direction += move.ReadValue<Vector2>().y * Get_Camera_Forward(player_Camera) * movement_Force;

        rb.AddForce(force_Direction, ForceMode.Impulse);
        force_Direction = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > max_Speed * max_Speed)
            rb.velocity = horizontalVelocity.normalized * max_Speed + Vector3.up * rb.velocity.y;

        Is_Grounded = Check_Grounded();
        Look_At();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        if (!move.enabled)
            return;

        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

}
