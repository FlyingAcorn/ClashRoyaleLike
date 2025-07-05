using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkeletonWarrior : Agent
{
    [SerializeField] private MeshCollider hitCollider;

    protected override IEnumerator Acting()
    {
        myAnimator.SetBool("isAttacking", true);
        hitCollider.enabled = true;
        transform.DOLookAt(target.transform.position, 0.25f, AxisConstraint.Y);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget() - target.ColliderOffset() > entityClassType.rangeRadius)
        {
            UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
        }
        else
        {
            currentCoroutine = StartCoroutine(Acting());
        }

        yield return null;
    }

    public void Hit()
    {
        hitCollider.enabled = false;
        myAnimator.SetBool("isAttacking", false);
    }
}