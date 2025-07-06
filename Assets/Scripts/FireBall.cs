using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class FireBall : Weapon
{
    [SerializeField] private ParticleSystem fireBallParticle;
    [SerializeField] private float radius;
    private Collider[] _entitiesInRange;
    [SerializeField] private LayerMask layerMask;
    public Tween CurrentTween;

    private void Awake()
    {
        _entitiesInRange = new Collider[30];
    }

    public override void OnHit(Entity target) // ObjectPooling ekle
    {
        Physics.OverlapSphereNonAlloc(target.transform.position, radius, _entitiesInRange, layerMask,
            QueryTriggerInteraction.Ignore);
        List<Collider> desiredList = _entitiesInRange.Where(c => c != null).ToList();
        foreach (var t in desiredList)
        {
            t.TryGetComponent(out Entity entity);
            if (entity.isAlly == owner.isAlly) continue;
            entity.Health -= owner.entityClassType.damage;
            entity.CheckHealth();
        }

        var spawnedEffect = EntityManager.Instance.explosionSfxPool.First();
        EntityManager.Instance.explosionSfxPool.Remove(spawnedEffect);
        spawnedEffect.gameObject.SetActive(true);
        spawnedEffect.transform.position = transform.position;
        spawnedEffect.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
        _entitiesInRange = new Collider[30];
    }

    private void OnDisable()
    {
        EntityManager.Instance.fireballPool.Add(this);
    }
}