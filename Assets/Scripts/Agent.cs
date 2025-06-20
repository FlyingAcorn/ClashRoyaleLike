using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

abstract class Agent : MonoBehaviour
{
   [SerializeField] private AgentScriptableObj agentClassType;
   [SerializeField] private NavMeshAgent myAgent;
   private enum AgentType
   {
      Enemy,
      Ally
   }
   private bool _isDead;
   private bool _isRanged;
   private int _health;
   private int _damage;
   private int _actSpeed;
   
   [SerializeField] private Agent target;

   private void Awake()
   {
       SetValues();
   }

   private void SetValues()
   {
       _isRanged = agentClassType.isRanged;
       _health = agentClassType.health;
       _damage = agentClassType.damage;
       _actSpeed = agentClassType.attackSpeed;
       myAgent.speed = agentClassType.speed;
       myAgent.stoppingDistance = agentClassType.stopDistance;
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
       Dead
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
            
       }

       if (newState == AgentBehaviour.Acting)
       {
       }

       if (newState == AgentBehaviour.Dead)
       {
            
       }
      
       OnAgentStateChanged?.Invoke(newState);
   }

   private IEnumerator AgentMovement()
   {
       yield return null;
   }

   private IEnumerator FindClosestTarget()
   {
       yield return null;
   }
}


