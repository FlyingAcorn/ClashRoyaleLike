using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Agent : Entity
{
   [SerializeField] private NavMeshAgent myAgent;

   private Coroutine _currentCoroutine;
   [SerializeField] private Entity target;
   

   protected override void Awake()
   {
       base.Awake();
       myAgent.speed = entityClassType.speed;
   }
   /*State machine yapacaksın mantığı
    Find Closest Target
    MoveTo Target (range icindeyse skip )
    Act  (diğer adıma geçmeden once actSpeed kadar bekleyecek)
    FindClosest Target
   -----------------------
   can 0 olursa bool true olacak ve her şeyi birakacak olme animasyonu yapıp olecek */
   
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
           _currentCoroutine = StartCoroutine(AgentMovement());
       }

       if (newState == AgentBehaviour.Acting)
       {
           _currentCoroutine = StartCoroutine(Acting());
       }

       if (newState == AgentBehaviour.Dying)
       {
           
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

   protected abstract IEnumerator Acting();

}


