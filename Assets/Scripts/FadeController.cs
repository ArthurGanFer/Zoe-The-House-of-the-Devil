using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private Animator fadeAnim;
    [SerializeField]
    private bool fadeIn;
    public bool animationFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnim = GetComponentInChildren<Animator>();
        
        if (fadeAnim != null)
        {
            if (fadeIn)
            {
                fadeAnim.SetBool("Fade-In", true);
            }
            else
            {
                fadeAnim.SetBool("Fade-In", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeAnim != null && fadeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            animationFinished = true;
        }
        
        if (animationFinished && fadeIn)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
