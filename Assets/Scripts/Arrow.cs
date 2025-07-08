using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : Weapon, IPoolable<Arrow>
{
    private Action<Arrow> returnToPool;

    public override void OnHit(Entity target)
    {
        target.Health -= owner.entityClassType.damage;
        target.CheckHealth();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ReturnToPool();
    }

    public void Initialize(Action<Arrow> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }
}