using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        if (!this.jumpScareAsset)
        {
            AssignComponents();
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
    }
}
