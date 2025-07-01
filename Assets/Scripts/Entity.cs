using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] public EntityScriptableObj entityClassType;
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected new Collider collider;
    [SerializeField] protected Entity target;
    private bool _isFlying;
    public bool isAlly;
    [SerializeField]private int health;

    public int Health
    {
        get => health;
        set => health = value > entityClassType.maxHealth ? entityClassType.maxHealth : value;
    }

    protected virtual void Awake()
    {
        _isFlying = entityClassType.isFlying;
        Health = entityClassType.maxHealth;
    }
    public void CheckHealth()
    {
        if (Health <= 0)
        { 
            DeathSequence();
            if (isAlly)
            {
                EntityManager.Instance.allies.Remove(this);
            }
            else
            {
                EntityManager.Instance.enemies.Remove(this);
            }
        }
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
    protected abstract void DeathSequence();
    public float ColliderOffset()
    {
        return collider.bounds.size.x*0.5f;
    }
}
