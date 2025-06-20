using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Agent : MonoBehaviour
{
   [SerializeField] private EntityScriptableObj entityClassType;
   [SerializeField] private NavMeshAgent myAgent;

   private Coroutine _currentCoroutine;
   private bool _isDead;
   private bool _isRanged;
   private int _health;
   private int _damage;
   private int _actSpeed;
   private float _rangeRadius;
   
   [SerializeField] private Entity target;
   

   private void Awake()
   {
       SetValues();
   }

   private void SetValues()
   {
       _isRanged = entityClassType.isRanged;
       _damage = entityClassType.damage;
       _actSpeed = entityClassType.attackSpeed;
       _rangeRadius = entityClassType.rangeRadius;
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
   }
   public static event Action<AgentBehaviour> OnAgentStateChanged;
   public AgentBehaviour currentBehaviour;
   
   private void Start()
   {
       UpdateGameState(AgentBehaviour.MovingToClosestTarget);
   }
    
   public void UpdateGameState(AgentBehaviour newState)
   {
       currentBehaviour = newState;
       
       if (newState == AgentBehaviour.MovingToClosestTarget)
       {
           _currentCoroutine = StartCoroutine(AgentMovement());
       }

       if (newState == AgentBehaviour.Acting)
       {
       }
       
       OnAgentStateChanged?.Invoke(newState);
   }

   private IEnumerator AgentMovement()
   {
       FindClosestTarget();
       var currentTarget = target;
       while (currentTarget == target || CheckAttackRange() )
       {
           myAgent.destination = target.transform.position;
           FindClosestTarget();
           yield return null;
       }
       
       yield return null;
   }

   private void FindClosestTarget()
   {
       Entity minDistanceEntity = null;
       var minDistance = Mathf.Infinity;
       foreach (var enemy in EntityManager.Instance.enemies)
       {
          float dis = Vector3.Distance(enemy.transform.position, transform.position);
           if (dis < minDistance)
           {
               minDistance = dis;
               minDistanceEntity = enemy;
           }
       }
       target = minDistanceEntity;
   }

   private bool CheckAttackRange()
   {

       return true; // şimdilik koydun
   }
}


