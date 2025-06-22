using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Agent : Entity
{
   [SerializeField] private NavMeshAgent myAgent;
   [SerializeField] private Rigidbody myBody;
   

   protected Coroutine currentCoroutine;
   [SerializeField] protected Entity target;

   protected override void Awake()
   {
       base.Awake();
       myAgent.speed = entityClassType.speed;
   }
   public enum AgentBehaviour
   {
       MovingToClosestTarget,
       Acting,
       Dying
   }
   public static event Action<AgentBehaviour> OnAgentStateChanged;
   public AgentBehaviour currentBehaviour;

   private void Start()
   {
       UpdateAgentState(AgentBehaviour.MovingToClosestTarget);
   }
    
   public void UpdateAgentState(AgentBehaviour newState)
   {
       currentBehaviour = newState;
       
       if (newState == AgentBehaviour.MovingToClosestTarget)
       {
           myAnimator.SetBool("isMoving",true);
           currentCoroutine = StartCoroutine(AgentMovement());
       }

       if (newState == AgentBehaviour.Acting)
       {
           myAnimator.SetBool("isMoving",false);
           currentCoroutine = StartCoroutine(Acting());
       }

       if (newState == AgentBehaviour.Dying)
       {
           StopCoroutine(currentCoroutine);
           myAgent.isStopped = true;
           myAnimator.SetTrigger("isDead");
           AnimatorClipInfo[] clipInfos = myAnimator.GetCurrentAnimatorClipInfo(0);
           var firstClipDuration = clipInfos[0].clip.averageDuration;
           DOVirtual.DelayedCall(firstClipDuration+2, (() => { gameObject.SetActive(false); }));
       }
       OnAgentStateChanged?.Invoke(newState);
   }

   private IEnumerator AgentMovement()
   {
       myAgent.isStopped = false;
       FindClosestTarget();
       while ( currentBehaviour == AgentBehaviour.MovingToClosestTarget )
       {
           myAgent.destination = target.transform.position;
           FindClosestTarget();
           if (FindClosestTarget()-target.ColliderOffset()<= entityClassType.rangeRadius)
           {
               myAgent.isStopped = true;
               UpdateAgentState( AgentBehaviour.Acting);
           }
           yield return null;
       }
       yield return null;
   }

   protected float FindClosestTarget()
   {
       Entity minDistanceEntity = null;
       var minDistance = Mathf.Infinity;
       
       foreach (var entity in isAlly ? EntityManager.Instance.enemies : EntityManager.Instance.allies)
       {
          float dis = Vector3.Distance(entity.transform.position, transform.position);
           if (dis < minDistance)
           {
               minDistance = dis;
               minDistanceEntity = entity;
           }
       }
       target = minDistanceEntity;
       return minDistance;
   }
   protected override void DeathSequence()
   {
       UpdateAgentState(AgentBehaviour.Dying);
   }
   protected abstract IEnumerator Acting();
  
}


