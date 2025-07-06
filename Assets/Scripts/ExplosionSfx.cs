using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSfx : MonoBehaviour
{
    private void OnDisable()
    {
        EntityManager.Instance.explosionSfxPool.Add(this);
    }
}