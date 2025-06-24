using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAssasin : Agent
{
    [SerializeField] private List<MeshCollider> hitColliders;
    [SerializeField] private Weapon projectile;

    protected override IEnumerator Acting()
    {
        throw new System.NotImplementedException();
    }

    public void Hit()
    {
        
    }
    public void Shoot()
    {
        
    }
}
