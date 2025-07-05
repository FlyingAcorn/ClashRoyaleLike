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
    [SerializeField] protected HealthBarCanvas healthBarCanvas;


    private bool _isFlying;
    public bool isAlly;
    [SerializeField] protected float health;
    protected Tweener HealthTween;
    protected Vector3 CameraPos;

    public virtual float Health
    {
        get => health;
        set
        {
            HealthTween.Kill();
            healthBarCanvas.healthBarSlider.gameObject.SetActive(true);
            HealthTween = healthBarCanvas.healthBarSlider.DOValue(health / entityClassType.maxHealth, 1);
            health = value > entityClassType.maxHealth ? entityClassType.maxHealth : value;
        }
    }

    protected virtual void Awake()
    {
        CameraPos = Camera.main.transform.position;
        _isFlying = entityClassType.isFlying;
        health = entityClassType.maxHealth;
        healthBarCanvas.healthBarSlider.direction = Slider.Direction.RightToLeft;
        if (isAlly) return;
        healthBarCanvas.healthBarColour.color = Color.red;
        healthBarCanvas.healthBarSlider.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    protected virtual void LateUpdate()
    {
        if (healthBarCanvas.healthBarSlider.gameObject.activeSelf)
        {
            healthBarCanvas.transform.rotation = Quaternion.LookRotation(new Vector3(transform.position.x,CameraPos.y,CameraPos.z)-transform.position);
        }
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
        return collider.bounds.size.x * 0.5f;
    }
}