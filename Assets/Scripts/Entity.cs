using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityScriptableObj entityClassType;

    private bool _isFlying;
    private int _maxHealth;
    private int _remainingHealth;
    public bool isAlly;

    private void Awake()
    {
        _isFlying = entityClassType.isFlying;
        _maxHealth = entityClassType.maxHealth;
    }


    private void OnTriggerEnter(Collider trigger)
    {
        
    }
}
