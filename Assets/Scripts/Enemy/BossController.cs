using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BossController : EnemyController
{
    [Header("Components")]
    private Animator targetAnim;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    private bool animSwitched = false;
    [SerializeField]
    private bool animSwitchTimerSet = false;    
    [SerializeField]
    private float baseAnimSwitchTimer = 4;    
    [SerializeField]
    private float currentAnimSwitchTimer;
    [SerializeField]
    private int previousRoute;


    // Start is called before the first frame update
    void Start()
    {
        if (!this.jumpScareAsset)
        {
            AssignComponents();

            this.targetAnim = this.target.GetComponent<Animator>();
            this.targetAnim.SetInteger("Route", 1);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.jumpScareAsset && this.target != null)
        {
            SearchForPlayer();

            if (this.playerInSight)
            {
                DetectionMeter();
            }
            else
            {
                this.chasingPlayer = false;
                this.timerSet = false;
                this.agent.speed = this.defaultSpeed;
                this.agent.SetDestination(this.target.position);
            }
        }

        if (this.enemyAnimator != null)
        {
            UpdateAnimations();
        }

        if (this.targetAnim != null)
        {
            BossUpdateAnimations();
        }
    }

    private void BossUpdateAnimations()
    {
        if (this.player.onCeiling)
        {
            this.StopCoroutine("AnimationSwitchTimer");
            this.targetAnim.SetInteger("Route", 2);

            this.targetAnim.SetBool("Loop", true);
        }
        else
        {
            this.targetAnim.SetBool("Loop", false);

            if (this.targetAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && this.targetAnim.GetInteger("Route") != 0)
            {
                this.previousRoute = this.targetAnim.GetInteger("Route");
                this.animSwitched = false;
                this.targetAnim.SetInteger("Route", 0);
            }

            if (!this.animSwitched)
            {
                this.StartCoroutine("AnimationSwitchTimer", baseAnimSwitchTimer);
            }
            else
            {
                this.StopCoroutine("AnimationSwitchTimer");
                this.animSwitchTimerSet = false;

                if (this.previousRoute == 1)
                {
                    this.targetAnim.SetInteger("Route", 2);
                }
                else if (this.previousRoute == 2)
                {
                    this.targetAnim.SetInteger("Route", 1);
                }
            }
        }
    }

    IEnumerator AnimationSwitchTimer(float baseTimer)
    {
        if (!this.animSwitchTimerSet)
        {
            this.currentAnimSwitchTimer = baseTimer;
            this.animSwitchTimerSet = true;
        }
        else
        {
            this.currentAnimSwitchTimer -= Time.deltaTime;

            if (this.currentAnimSwitchTimer <= 0)
            {
                Debug.Log("Route Switched!");

                yield return this.animSwitched = true;
            }
        }
    }
}
