using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Agent
{
    [SerializeField] private MeshCollider hitCollider;
    
    protected override IEnumerator Acting()
    {
        while (currentBehaviour == AgentBehaviour.Acting)
        {
            //Play anim
            hitCollider.enabled = true;
            yield return new WaitForSeconds(1);
            // wait for anim to finish
            hitCollider.enabled = false;
            yield return new WaitForSeconds(1);
            
            if (FindClosestTarget()> entityClassType.rangeRadius)
            {
                UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
            }
            else
            {
                yield return null;
            }
        }
        yield return null;
    }
}
