using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireBall : Weapon
{
    [SerializeField] private ParticleSystem fireBallParticle;
    [SerializeField] private float radius;
    private Collider[] _entitiesInRange;

    private void Awake()
    {
        _entitiesInRange = new Collider[30];
    }

    protected override void OnHit(Entity target) // beceremedin buna cozum bul
    {
        var spawnedEffect = Instantiate(fireBallParticle); // wizard owner atadığında particle effect child objesi yok oluyor.
        spawnedEffect.Play();
        var size = Physics.OverlapSphereNonAlloc(target.transform.position, radius, _entitiesInRange);
        Debug.Log(size);
        List<Collider> desiredList = _entitiesInRange.Where(c =>c !=null &&c.isTrigger == false && c.TryGetComponent(out Entity _)).ToList(); // bunu findall cevir
        Debug.Log(desiredList.Count);
        for (int i = 0; i < desiredList.Count; i++)
        {
            desiredList[i].TryGetComponent(out Entity entity);
            if (entity.isAlly == owner.isAlly) return;
            entity.Health -= owner.entityClassType.damage;
        }
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
