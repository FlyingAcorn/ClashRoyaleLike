using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSfx : MonoBehaviour, IPoolable<ExplosionSfx>
{
    private Action<ExplosionSfx> returnToPool;

    private void OnDisable()
    {
        ReturnToPool();
    }

    public void Initialize(Action<ExplosionSfx> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }
}