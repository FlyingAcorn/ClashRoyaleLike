using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] public EntityScriptableObj entityClassType;
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected new Collider collider;
    [SerializeField] protected Entity target;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Image healthBarImage;
    
    
    private bool _isFlying;
    public bool isAlly;
    [SerializeField]private float health;

    public float Health
    {
        get => health;
        set
        {
            healthBarSlider.gameObject.SetActive(true);
            healthBarSlider.DOValue(health / entityClassType.maxHealth, 1)/*.OnComplete((() =>
            {
                DOVirtual.DelayedCall(1, (() => healthBarSlider.gameObject.SetActive(false))); }))*/; 
            health = value > entityClassType.maxHealth ? entityClassType.maxHealth : value;
        }
    }

    protected virtual void Awake()
    {
        _isFlying = entityClassType.isFlying;
        health = entityClassType.maxHealth;
        healthBarImage.color = isAlly ? healthBarImage.color : Color.red;
    }

    protected void Start()
    {
        
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
