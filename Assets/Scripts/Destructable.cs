using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private int hitPoints;
    public GameObject particlePrefab;
    [SerializeField]
    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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

                Destroy(this.gameObject);
            }
        }
    }
}
