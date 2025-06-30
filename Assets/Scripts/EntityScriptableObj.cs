using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityInfo",menuName = "ScriptableObj/EntityInfo")]
public class EntityScriptableObj : ScriptableObject
{
    public bool isFlying;
    public bool isRanged;
    public int maxHealth;
    public int damage;
    public float attackSpeed;
    public float speed;
    public float rangeRadius;
}

