using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class grappleMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ThirdPersonActionsAsset thirdPersonActionAsset;
    [SerializeField]
    private GameObject hookPrefab;
    [SerializeField]
    private Transform shootTransform;
    [SerializeField]
    private hookMechanic hookMechanic;
    [SerializeField]
    private Rigidbody playerRB;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    private float pullSpeed = 0.5f;
    [SerializeField]
    private float stopDistance = 4f;
    [SerializeField]
    private bool pulling;
    [SerializeField]
    private bool useGrapple;

    private void Start()
    {
        AssignComponents();
        thirdPersonActionAsset.Player.UseItem.started += UseGrapple;
        pulling = false;
        useGrapple = false;
    }

    private void OnDisable()
    {
        thirdPersonActionAsset.Player.UseItem.started -= UseGrapple;
    }

    private void Update()
    {
        if (useGrapple)
        {
            if (hookMechanic == null)
            {
                StopCoroutine(DestroyHookAfterLifetime());
                pulling = false;
                hookMechanic = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<hookMechanic>();
                hookMechanic.InitializeGrapple(this, shootTransform);
                StartCoroutine(DestroyHookAfterLifetime());
            }

            if (!pulling || hookMechanic == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, hookMechanic.transform.position) <= stopDistance)
            {
                DestroyHook();
            }
            else
            {
                playerRB.AddForce((hookMechanic.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
            }
        }
        else if (useGrapple && hookMechanic != null)
        {
            DestroyHook();
        }
    }

    private void AssignComponents()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
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
            playerRB = playerController.rb;
            if (playerRB == null)
            {
                Debug.Log($"There is no Rigidbody Component on {this} GameObject!");
            }
        }

    }

    private void UseGrapple(InputAction.CallbackContext obj)
    {
        useGrapple = true;
        Debug.Log("Used Grapple!");
    }

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hookMechanic == null)
        {
            return;
        }

        pulling = false;
        Destroy(hookMechanic.gameObject);
        hookMechanic = null;
        useGrapple = false;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }
}
