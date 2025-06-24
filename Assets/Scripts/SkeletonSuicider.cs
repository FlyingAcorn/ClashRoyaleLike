using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSuicider : Agent
{
    [SerializeField] private ParticleSystem explosionParticle;
    protected override IEnumerator Acting()
    {
        throw new System.NotImplementedException();
    }

    public void BlowUp()
    {
        
    }
}
