using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Wizard : Agent
{
    private Entity _attackTarget;
    private float _attackTargetColliderOffset;
    private Vector3 _attackTargetPos;

    protected override IEnumerator Acting()
    {
        _attackTarget = target;
        _attackTargetColliderOffset = target.ColliderOffset();
        _attackTargetPos = target.transform.position;
        myAnimator.SetBool("isAttacking", true);
        transform.DOLookAt(_attackTargetPos, 0.25f, AxisConstraint.Y);
        yield return new WaitForSeconds(2.20f);
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget() - _attackTargetColliderOffset > entityClassType.rangeRadius)
        {
            UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
        }
        else
        {
            currentCoroutine = StartCoroutine(Acting());
        }

        yield return null;
    }

    public void Shoot() // animEvent
    {
        var fireball = PoolManager.Instance.fireBallPool.Pull(transform.position + new Vector3(0, 1, 0));
        fireball.owner = this;
        var time = FindClosestTarget() / 30; // 20 is speedper pixel
        var targetsPos = _attackTargetPos;
        fireball.transform.DOMove(targetsPos + new Vector3(0, 1, 0), time).OnComplete(() =>
        {
            if (!_attackTarget) return;
            fireball.OnHit(_attackTarget);
        });
        myAnimator.SetBool("isAttacking", false);
    }
}