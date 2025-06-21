using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected EntityScriptableObj entityClassType;

    private bool _isFlying;
    public bool isAlly;
    private int Health
    {
        get => Health;
        set => Health = Health+value > entityClassType.maxHealth ? entityClassType.maxHealth : value;
    }

    private void Awake()
    {
        _isFlying = entityClassType.isFlying;
        
    }


    private void OnTriggerEnter(Collider trigger)
    {
        
    }
}
