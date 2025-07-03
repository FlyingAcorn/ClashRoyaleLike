using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Wizard : Agent
{
    [SerializeField] private Weapon projectile;
    
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
    public void Shoot() // animEvent
    {
        var fireball = Instantiate(projectile, transform.position+new Vector3(0,1,0),Quaternion.identity);
        fireball.owner=this;
        var time = FindClosestTarget() / 30; // 20 is speedper pixel
        var _targetsPos = target.transform.position;
        fireball.transform.DOMove(_targetsPos+new Vector3(0, 1, 0),time).OnComplete(() => fireball.OnHit(target));;
        myAnimator.SetBool("isAttacking",false);
    }
}
