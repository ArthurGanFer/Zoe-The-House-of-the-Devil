using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlumberController : PlayerController
{
    [Header("Components")]
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Animator wrenchAnim;
    
    protected override void AssignComponents()
    {
        base.AssignComponents();
        /*
        if (wrenchAnim == null)
        {
            wrenchAnim = GetComponent<Animator>();
        }
        if (wrenchAnim == null)
        {
            Debug.LogError("[WrenchMechanic] No Animator component found!");
        }
        */
    }

    protected override void UnassignComponents()
    {
        base.UnassignComponents();
        wrenchAnim = null;
    }

    protected override void Use_Item(InputAction.CallbackContext obj)
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
    
    protected override void Do_Possess(InputAction.CallbackContext obj)
    {
        Debug.Log("Not main character");
    }
    
}
