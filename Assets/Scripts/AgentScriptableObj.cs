using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent",menuName = "ScriptableObj/Agent")]
public class AgentScriptableObj : ScriptableObject
{
    
    public bool isRanged;
    public int health;
    public int damage;
    public int attackSpeed;
    public int speed;
    public int stopDistance;
}
