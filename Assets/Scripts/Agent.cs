using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Agent : Entity
{
   [SerializeField] private NavMeshAgent myAgent;
   [SerializeField] private Rigidbody myBody;
   

   private Coroutine _currentCoroutine;
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
           _currentCoroutine = StartCoroutine(AgentMovement());
       }

       if (newState == AgentBehaviour.Acting)
       {
           myAnimator.SetBool("isMoving",false);
           _currentCoroutine = StartCoroutine(Acting());
       }

       if (newState == AgentBehaviour.Dying)
       {
           myAgent.isStopped = true;
           myAnimator.SetTrigger("isDead");
           //timer ekle ve sil
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
           if (FindClosestTarget()<= entityClassType.rangeRadius)
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


