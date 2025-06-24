using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMinion : Agent
{
    [SerializeField] private MeshCollider hitCollider;
    protected override IEnumerator Acting()
    {
        throw new System.NotImplementedException();
    }
    public void Hit()
    {
       
    }
}
