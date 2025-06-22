using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Entity owner;
    [SerializeField] private Collider myCollider;
    

    public void OnHit()
    {
        gameObject.SetActive(false);
    }

}
