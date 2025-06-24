using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Agent
{
    [SerializeField] private List<Entity> summons;

    protected override IEnumerator Acting()
    {
        throw new System.NotImplementedException();
    }

    public void Summon()
    {
        
    }
}
