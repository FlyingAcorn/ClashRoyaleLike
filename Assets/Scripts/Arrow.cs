using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : Weapon
{
    public override void OnHit(Entity target)
    {
        target.Health -= owner.entityClassType.damage;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}