using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Entity owner;
    [SerializeField] private Collider myCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (owner.entityClassType.isRanged )return;
        if (other.TryGetComponent(out Entity target) && owner.isAlly !=target.isAlly)
        {
            OnHit(target);
            //target.myAnimator.SetTrigger("isHit"); animator public olacak
            target.CheckHealth();
            
        }
    }

    public abstract void OnHit(Entity target);
}
