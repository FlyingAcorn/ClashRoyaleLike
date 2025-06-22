using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityScriptableObj entityClassType;
    [SerializeField] protected Animator myAnimator;

    private bool _isFlying;
    public bool isAlly;
    [SerializeField]private int health;

    private int Health
    {
        get => health;
        set { health = value > entityClassType.maxHealth ? entityClassType.maxHealth : value; }
    }

    protected virtual void Awake()
    {
        _isFlying = entityClassType.isFlying;
        Health = entityClassType.maxHealth;
        Debug.Log(Health);

    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.TryGetComponent(out Weapon weapon) && weapon.owner.isAlly != isAlly)
        {
            //myAnimator.SetTrigger("isHit");
            Health -= weapon.owner.entityClassType.damage;
            weapon.owner.GotHitSequence();
            Debug.Log(weapon.owner.gameObject.name+"  "+gameObject.name);
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
    }
    protected abstract void DeathSequence();
    protected abstract void GotHitSequence();
}
