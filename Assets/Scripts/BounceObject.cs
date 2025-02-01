using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BounceObject : MonoBehaviour
{

    [SerializeField] float bounceForce = 1f;

    public GameObject bouncyObject;

    public Animator ballAnimator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                Vector3 bounceUp = Vector3.up;
                playerRigidbody.AddForce(bounceUp * bounceForce, ForceMode.Impulse);

                if (bouncyObject != null)
                {
                    ballAnimator.SetBool("Bounce", true);

                    StartCoroutine(ResetBool());
                }

            }


        }


    }


    private IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(0.5f);

        if (ballAnimator != null)
        {
            ballAnimator.SetBool("Bounce", false);
        }
    }

}
