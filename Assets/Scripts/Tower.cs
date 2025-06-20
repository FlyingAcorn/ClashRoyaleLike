using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private EntityScriptableObj entityClassType;
    
    private bool _isDead;
    private int _damage;
    private int _actSpeed;
    private float _rangeRadius;

    private void Awake()
    {
        SetValues();
    }
    
    private void SetValues()
    {
        _damage = entityClassType.damage;
        _actSpeed = entityClassType.attackSpeed;
        _rangeRadius = entityClassType.rangeRadius;
    }
    
    /* find the closest target in range
    act*/
}
