using System.Collections;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private int hitPoints;
    public GameObject particlePrefab;
    [SerializeField]
    private Animator anim;

    [SerializeField] private GameObject wholeObject;
    [SerializeField] GameObject[] brokenParts;

    public float DestroyAfterSeconds = 5f;

    public float pulseDistance = 1f;

    public AudioClip breakSound;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim != null)
        {
            if (hitPoints > 1)
            {
                anim.SetBool("Broken", false);
            }
            else
            {
                anim.SetBool("Broken", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            if (hitPoints > 1)
            {
                hitPoints = hitPoints - 1;
            }
            else
            {
                if (particlePrefab != null)
                {
                    GameObject temp = GameObject.Instantiate(particlePrefab);
                    temp.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                }

                BreakObject();
            }
        }
    }

    private void BreakObject()
    {
        if (wholeObject != null)
        {
            MeshRenderer meshRenderer = wholeObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            Collider collider = wholeObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            Rigidbody rb = wholeObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }


            // Destroy the whole object after 6f;

            StartCoroutine(DestroyWholeObject());

            if (brokenParts != null && brokenParts.Length > 0)
            {
                foreach (GameObject part in brokenParts)
                {
                    if (part != null)
                    {
                        part.transform.SetParent(null);
                        part.SetActive(true);

                        MeshRenderer partRenderer = part.GetComponent<MeshRenderer>();
                        if (partRenderer != null)
                        {
                            partRenderer.enabled = true;
                        }

                        Collider partCollider = part.GetComponent<Collider>();
                        if (partCollider != null)
                        {
                            partCollider.enabled = true;
                        }

                        Rigidbody partRb = part.GetComponent<Rigidbody>();
                        if (partRb != null)
                        {
                            partRb.isKinematic = false;
                            ApplyPulseForce(part, partRb);
                        }
                    }
                }

            // Destroy parts of the object after 5f;

                StartCoroutine(DestroyPiecesAfterDelay());
            }

            if (breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, transform.position);
            }

        }
    }


    //Apply a impulse force in the shards.
    private void ApplyPulseForce(GameObject part, Rigidbody partRb)
    {
        Vector3 hitDirection = (part.transform.position - transform.position).normalized;
        partRb.AddForce(hitDirection * pulseDistance, ForceMode.Impulse);
    }

    private IEnumerator DestroyPiecesAfterDelay()
    {
        yield return new WaitForSeconds(DestroyAfterSeconds);

        if (brokenParts != null && brokenParts.Length > 0)
        {
            foreach (GameObject part in brokenParts)
            {
                if (part != null)
                {
                    Destroy(part);
                }
            }
        }
    }

    private IEnumerator DestroyWholeObject()
    {
        yield return new WaitForSeconds(6f);

        if (wholeObject != null)
        {
            Destroy(wholeObject);
        }
    }
}
