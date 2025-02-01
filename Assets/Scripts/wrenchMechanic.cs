using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WrenchMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Animator wrenchAnim;
    [SerializeField] private ThirdPersonActionsAsset thirdPersonActionAsset;

    private void Start()
    {
        AssignComponents();

        // Ensure Input System is properly handled
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

        if (thirdPersonActionAsset != null)
        {
            thirdPersonActionAsset.Player.UseItem.started += UseWrench;
        }
        else
        {
            Debug.LogError("[WrenchMechanic] ThirdPersonActionsAsset is not assigned!");
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("[WrenchMechanic] Collider disabled at start.");
        }
        else
        {
            Debug.LogError("[WrenchMechanic] Attack Collider is missing! Assign it in the Inspector.");
        }
    }

    private void OnDisable()
    {
        if (thirdPersonActionAsset != null)
        {
            thirdPersonActionAsset.Player.UseItem.started -= UseWrench;
        }
    }

    private void AssignComponents()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.LogError("[WrenchMechanic] No PlayerController found in the scene!");
        }
        else
        {
            thirdPersonActionAsset = playerController.player_Action_Asset;
        }

        if (wrenchAnim == null)
        {
            wrenchAnim = GetComponent<Animator>();
        }
        if (wrenchAnim == null)
        {
            Debug.LogError("[WrenchMechanic] No Animator component found!");
        }
    }

    private void UseWrench(InputAction.CallbackContext obj)
    {
        if (wrenchAnim != null)
        {
            wrenchAnim.SetBool("Attack", true);
        }

        StartCoroutine(WrenchCoroutine());
    }

    private IEnumerator WrenchCoroutine()
    {
        Debug.Log("[WrenchMechanic] Wrench attack started!");

        if (wrenchAnim != null)
        {
            wrenchAnim.SetBool("WrenchAttack", true);
        }

        yield return new WaitForSeconds(0.4f); // Wait time before enabling collider

        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            Debug.Log("[WrenchMechanic] ATTACK COLLIDER ENABLED");
        }
        else
        {
            Debug.LogError("[WrenchMechanic] Attack Collider is NULL! Make sure it is assigned.");
        }

        yield return new WaitForSeconds(0.2f); // Duration of the attack

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("[WrenchMechanic] ATTACK COLLIDER DISABLED");
        }

        if (wrenchAnim != null)
        {
            wrenchAnim.SetBool("WrenchAttack", false);
        }

        Debug.Log("[WrenchMechanic] Wrench attack finished.");
    }

    // Optional: Use an animation event to enable the collider at the right time.
    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            Debug.Log("[WrenchMechanic] ATTACK COLLIDER ENABLED (via animation event)");
        }
    }

    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("[WrenchMechanic] ATTACK COLLIDER DISABLED (via animation event)");
        }
    }
}
