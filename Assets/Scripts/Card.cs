using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardInfo;
    public List<Entity> entities;
    private void Awake()
    {
        foreach (var t in entities)
        {
            EntityManager.Instance.AddEntity(t);
        } 
    }
}
