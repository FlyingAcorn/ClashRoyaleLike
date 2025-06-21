using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected EntityScriptableObj entityClassType;

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
        if (trigger.transform.TryGetComponent(out Weapon weapon))
        {
            Health -= weapon.owner.entityClassType.damage;
            if (Health <= 0)
            {
                Debug.Log("dead"+gameObject.name);
                if (isAlly)
                {
                    EntityManager.Instance.allies.Remove(this);
                }
                else
                {
                    EntityManager.Instance.enemies.Remove(this);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
