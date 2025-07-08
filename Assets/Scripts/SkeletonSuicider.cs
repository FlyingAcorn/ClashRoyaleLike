using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkeletonSuicider : Agent
{
    [SerializeField] private float explosionRadius;
    private Collider[] _entitiesInRange;
    [SerializeField] private LayerMask layerMask;


    protected override void Awake()
    {
        base.Awake();
        _entitiesInRange = new Collider[30];
    }

    protected override IEnumerator Acting()
    {
        myAnimator.SetBool("isAttacking", true);
        transform.DOLookAt(target.transform.position, 0.25f, AxisConstraint.Y);
        yield return new WaitForSeconds(3);
        Health = 0;
        CheckHealth();
        yield return null;
    }

    public void BlowUp()
    {
        Physics.OverlapSphereNonAlloc(transform.position + new Vector3(0, 1, 0), explosionRadius,
            _entitiesInRange, layerMask, QueryTriggerInteraction.Ignore);
        List<Collider> desiredList = _entitiesInRange.Where(c => c != null).ToList();
        foreach (var t in desiredList)
        {
            t.TryGetComponent(out Entity entity);
            if (entity.isAlly == isAlly) continue;
            entity.Health -= entityClassType.damage;
            entity.CheckHealth();
        }

        var spawnedEffect = PoolManager.Instance.explosionSfxPool.Pull(transform.position);
        spawnedEffect.gameObject.SetActive(true);
        _entitiesInRange = new Collider[30];
        myAnimator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), explosionRadius);
    }
}