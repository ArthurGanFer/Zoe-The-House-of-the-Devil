    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Animator lightAnim;
    public SphereCollider lightCol;
    [SerializeField]
    private Light lightComponent;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    private float deltaTimer;    
    [SerializeField]
    private float timer;
    [SerializeField]
    public bool lightOn;

    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLightRange();
        if (lightOn)
        {
            RunTimer();
        }
        else
        {
            ResetTimer();
        }
    }

    private void AssignComponents()
    {
        lightAnim = GetComponent<Animator>();
        if (lightAnim == null)
        {
            Debug.Log($"There is no Animator Component on {this} GameObject!");
        }
        lightCol = GetComponent<SphereCollider>();
        if (lightCol == null)
        {
            Debug.Log($"There is no Collider Component on {this} GameObject!");
        }
        if (lightComponent == null)
        {
            Debug.Log($"There is no Light Component on {this} GameObject!");
        }

        timer = deltaTimer;
    }

    private void UpdateLightRange()
    {
        lightComponent.range = lightCol.radius;
    }

    private void RunTimer()
    {
        lightCol.enabled = true;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;

            lightAnim.SetBool("isShrinking", true);

            if (lightCol.GetComponent<SphereCollider>().radius <= 0.05f)
            {
                lightOn = false;
            }
        }
    }

    private void ResetTimer()
    {
        lightCol.enabled = false;
        lightAnim.SetBool("isShrinking", false);

        timer = deltaTimer;
    }
}
