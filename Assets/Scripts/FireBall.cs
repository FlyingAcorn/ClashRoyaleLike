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

    public override void OnHit(Entity target) // beceremedin buna cozum bul
    {
        Physics.OverlapSphereNonAlloc(target.transform.position, radius, _entitiesInRange, layerMask,
            QueryTriggerInteraction.Ignore);
        List<Collider> desiredList = _entitiesInRange.Where(c => c != null).ToList();
        Debug.Log(desiredList.Count); // bunu findall cevir
        foreach (var t in desiredList)
        {
            Debug.Log("anan");
            t.TryGetComponent(out Entity entity);
            if (entity.isAlly == owner.isAlly) continue;
            Debug.Log("hit" + entity.name);
            entity.Health -= owner.entityClassType.damage;
            entity.CheckHealth();
        }

        var spawnedEffect =
            Instantiate(fireBallParticle, target.transform.position,
                Quaternion.identity); // wizard owner atadığında particle effect child objesi yok oluyor.
        spawnedEffect.Play();
        gameObject.SetActive(false);
        _entitiesInRange = new Collider[30];
        //Destroy(gameObject);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }*/
}