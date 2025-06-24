using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Rogue : Agent
{
    [SerializeField] private List<MeshCollider> hitColliders;
    
    protected override IEnumerator Acting()
    {
        myAnimator.SetBool("isAttacking",true);
        transform.DOLookAt(target.transform.position,0.25f,AxisConstraint.Y);
        yield return new WaitForSeconds(2.5f);
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget()-target.ColliderOffset()> entityClassType.rangeRadius)
        {
            UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
        }
        else
        {
            currentCoroutine = StartCoroutine(Acting());
        }
        yield return null;
    }

    public void Hit()//AnimEvent
    {
        foreach (var c in hitColliders) c.enabled = !c.enabled;
        myAnimator.SetBool("isAttacking",false);
    }
}
