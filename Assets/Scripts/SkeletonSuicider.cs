using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkeletonSuicider : Agent
{
    [SerializeField] private ParticleSystem explosionParticle;
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
        // .Where(c => c != null && c.isTrigger == false && c.TryGetComponent(out Entity _)).ToList();
        Debug.Log(desiredList.Count);
        foreach (var t in desiredList)
        {
            Debug.Log("anan");
            t.TryGetComponent(out Entity entity);
            if (entity.isAlly == isAlly) continue;
            Debug.Log("hit" + entity.name);
            entity.Health -= entityClassType.damage;
            entity.CheckHealth();
        }

        var spawnedEffect =
            Instantiate(explosionParticle, transform.position,
                Quaternion.identity); // Fireballdakinin aynısı değiştirebilirsin obejctpooling vs olunca
        spawnedEffect.Play();
        _entitiesInRange = new Collider[30];
        myAnimator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), explosionRadius);
    }
}