using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class wrenchMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Collider attackCollider;
    [SerializeField]
    private Animator wrenchAnim;
    [SerializeField]
    private ThirdPersonActionsAsset thirdPersonActionAsset;

    private void Awake()
    {
        AssignComponents();
    }

    private void OnEnable()
    {
        thirdPersonActionAsset.Player.UseItem.started += UseWrench;
    }

    private void OnDisable()
    {
        thirdPersonActionAsset.Player.UseItem.started -= UseWrench;
    }

    private void AssignComponents()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null )
        {
            Debug.Log("There is no object with a PlayerController Component in the scene!");
        }
        else
        {
            thirdPersonActionAsset = playerController.player_Action_Asset;
            if (thirdPersonActionAsset == null)
            {
                Debug.Log($"There is no thirdPersonActionAsset Component on {this} GameObject!");
            }
        }
        
        wrenchAnim = GetComponent<Animator>();
        if ( wrenchAnim == null )
        {
            Debug.Log($"There is no Animator Component on {this} GameObject!");
        }
        
    }

    private void UseWrench(InputAction.CallbackContext obj)
    {
        StartCoroutine(WrenchCoroutine());
    }

    IEnumerator WrenchCoroutine()
    {
        wrenchAnim.SetBool("WrenchAttack", true);

        yield return new WaitForSeconds(0.5f);
        
        wrenchAnim.SetBool("WrenchAttack", false);

        yield return null;

        StopCoroutine();
    }

    private void StopCoroutine()
    {
        StopCoroutine(WrenchCoroutine());
    }
}
