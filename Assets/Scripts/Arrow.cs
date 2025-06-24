using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Weapon
{
    protected override void OnHit(Entity target)
    {
        gameObject.SetActive(false);
        target.Health -= owner.entityClassType.damage;
    }
}
