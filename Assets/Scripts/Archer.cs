using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Archer : Agent
{
    [SerializeField] private Weapon projectile;
    private float _attackTargetColliderOffset;
    private Vector3 _attackTargetPos;
    private Entity _attackTarget;

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
        var direction = _attackTargetPos - transform.position + new Vector3(0, 10, 0); // y değeri offset
        var arrow = Instantiate(projectile, transform.position + new Vector3(0, 1, 0),
            Quaternion.LookRotation(direction));
        arrow.owner = this;
        var time = FindClosestTarget() / 30; // 20 is speedper pixel
        var _targetsPos = _attackTargetPos;

        arrow.transform.DOMove(_targetsPos + new Vector3(0, 1, 0), time)
            .OnComplete(() =>
            {
                if (!_attackTarget) return;
                arrow.OnHit(_attackTarget);
            });
        myAnimator.SetBool("isAttacking", false);
    }
}