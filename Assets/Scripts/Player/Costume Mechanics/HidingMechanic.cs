using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HidingMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("A reference to our PlayerController component")]
    private PlayerController playerController;

    [Space(10)]

    [Header("Properties")]
    [SerializeField]
    [Tooltip("A string representing the layer where we keep our hiding spots")]
    private string layer = "HidingSpot";
    [SerializeField]
    [Tooltip("A flag to keep track of whether we are hidden or not")]
    public bool isHidden = false;


    private void Start()
    {
        AssignComponents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HidingSpot") && this.playerController.Main_Character)
        {
            if (this.playerController.is_crouching)
            {
                isHidden = true;
            }
            else
            {
                isHidden = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layer) && this.playerController.Main_Character)
        {
            if (this.playerController.is_crouching)
            {
                this.isHidden = true;
            }
            else
            {
                this.isHidden = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            this.isHidden = false;
        }
    }

    private void AssignComponents()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null )
        {
            Debug.Log($"There is no PlayerController component on {this} GameObject!");
        }
    }
}
