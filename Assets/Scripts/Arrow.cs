using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : Weapon
{
    public Tween CurrentTween;
    protected override void OnHit(Entity target)
    {
        target.Health -= owner.entityClassType.damage;
        CurrentTween.Kill();
        Destroy(gameObject);
    }
}
