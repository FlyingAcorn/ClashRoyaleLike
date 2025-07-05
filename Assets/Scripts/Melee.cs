using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Melee : Weapon
{
    private Entity _frameAgainst;

    public override void OnHit(Entity target)
    {
        if (owner != _frameAgainst)
        {
            target.Health -= owner.entityClassType.damage;
            _frameAgainst = owner;
            DOVirtual.DelayedCall(0.3f, () => { _frameAgainst = null; });
        }
    }
}