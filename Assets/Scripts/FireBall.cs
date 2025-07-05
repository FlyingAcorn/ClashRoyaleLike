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
    public Tween CurrentTween;

    private void Awake()
    {
        _entitiesInRange = new Collider[30];
    }

    public override void OnHit(Entity target) // beceremedin buna cozum bul
    {
        var spawnedEffect =
            Instantiate(fireBallParticle, target.transform.position,
                Quaternion.identity); // wizard owner atadığında particle effect child objesi yok oluyor.
        spawnedEffect.Play();
        var size = Physics.OverlapSphereNonAlloc(target.transform.position, radius, _entitiesInRange);
        List<Collider> desiredList = _entitiesInRange
            .Where(c => c != null && c.isTrigger == false && c.TryGetComponent(out Entity _))
            .ToList(); // bunu findall cevir
        for (int i = 0; i < desiredList.Count; i++)
        {
            desiredList[i].TryGetComponent(out Entity entity);
            if (entity.isAlly == owner.isAlly) return;
            Debug.Log(entity.name + entity.Health);
            entity.Health -= owner.entityClassType.damage;
            entity.CheckHealth();
            Debug.Log(entity.name + entity.Health);
        }

        gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }*/
}