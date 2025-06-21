using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Knight : Agent
{
    [SerializeField] private MeshCollider hitCollider;
    
    protected override IEnumerator Acting()
    {
        while (currentBehaviour == AgentBehaviour.Acting)
        {
            myAnimator.SetBool("isAttacking",true);
            hitCollider.enabled = true;
            transform.DOLookAt(target.transform.position,2,AxisConstraint.Y);
            AnimatorClipInfo[] clipInfos = myAnimator.GetCurrentAnimatorClipInfo(0);
            var firstClipDuration = clipInfos[0].clip.averageDuration;
            yield return new WaitForSeconds(firstClipDuration);
            myAnimator.SetBool("isAttacking",false);
            hitCollider.enabled = false;
            yield return new WaitForSeconds(entityClassType.attackSpeed);
            if (FindClosestTarget()> entityClassType.rangeRadius && currentBehaviour != AgentBehaviour.Dying)
            {
                UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
            }
            else
            {
                yield return null;
            }
        }
        yield return null;
    }
}
