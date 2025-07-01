using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Necromancer : Agent
{
    [SerializeField] private List<Entity> summons;
    [SerializeField] private LayerMask layerMask;
    

    protected override IEnumerator Acting()
    {
        myAnimator.SetBool("isAttacking",true);
        transform.DOLookAt(target.transform.position,0.25f,AxisConstraint.Y);
        yield return new WaitForSeconds(2.20f);
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget()-target.ColliderOffset()> entityClassType.rangeRadius)
        {
            UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
        }
        else
        {
            currentCoroutine = StartCoroutine(Acting());
        }
        yield return null;
    }

    public void Summon()
    {
        foreach (var summon in summons)
        {
            summon.isAlly = isAlly;
           Entity summoned = Instantiate(summon.gameObject,CheckEmptySpace(),Quaternion.identity).GetComponent<Entity>();
            EntityManager.Instance.AddEntity(summoned);
        }
        myAnimator.SetBool("isAttacking",false);
    }

    private Vector3 CheckEmptySpace()
    {
        var xPos = Random.Range(transform.position.x - entityClassType.rangeRadius*0.5f,
            transform.position.x + entityClassType.rangeRadius*0.5f);
        var zPos =Random.Range(transform.position.z - entityClassType.rangeRadius*0.5f,
            transform.position.z + entityClassType.rangeRadius*0.5f); 
        var location = new Vector3(xPos, transform.position.y, zPos);
        while (Physics.CheckSphere(location,1,layerMask))
        {
            xPos = Random.Range(transform.position.x - entityClassType.rangeRadius*0.5f,
                transform.position.x + entityClassType.rangeRadius*0.5f);
            zPos =Random.Range(transform.position.z - entityClassType.rangeRadius*0.5f,
                transform.position.z + entityClassType.rangeRadius*0.5f); 
            location = new Vector3(xPos, transform.position.y, zPos);
        }
        Debug.Log(location);
        return location;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+new Vector3(0,0,0),entityClassType.rangeRadius*0.5f);
    }
}
