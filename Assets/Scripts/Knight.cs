using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Knight : Agent
{
    [SerializeField] public MeshCollider hitCollider;
    
    protected override IEnumerator Acting()
    {
        myAnimator.SetBool("isAttacking",true);
        hitCollider.enabled = true;
        AnimatorClipInfo[] clipInfos = myAnimator.GetCurrentAnimatorClipInfo(0);
        var firstClipDuration = clipInfos[0].clip.averageDuration;
        transform.DOLookAt(target.transform.position,firstClipDuration*0.5f,AxisConstraint.Y);
        yield return new WaitForSeconds(firstClipDuration*0.5f);
        myAnimator.SetBool("isAttacking",false);
        hitCollider.enabled = false;
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget()> entityClassType.rangeRadius)
        {
            UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
        }
        else
        {
           currentCoroutine = StartCoroutine(Acting());
        }
        yield return null;
    }

    protected override void GotHitSequence()
    {
        myAnimator.SetBool("isAttacking",false);
        hitCollider.enabled = false;
    }
}
