using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    public Arrow arrowPrefab;
    public FireBall fireballPrefab;
    public ExplosionSfx explosionSfxPrefab;
    public ObjectPool<Arrow> arrowPool;
    public ObjectPool<FireBall> fireBallPool;
    public ObjectPool<ExplosionSfx> explosionSfxPool;
    [SerializeField] private GameObject poolsParentObj;


    private void OnEnable()
    {
        arrowPool = new ObjectPool<Arrow>(arrowPrefab.gameObject, 30, poolsParentObj);
        fireBallPool = new ObjectPool<FireBall>(fireballPrefab.gameObject, 10, poolsParentObj);
        explosionSfxPool = new ObjectPool<ExplosionSfx>(explosionSfxPrefab.gameObject, ExplosionCallOnPull, null, 10,
            poolsParentObj);
    }

    private void ExplosionCallOnPull(ExplosionSfx explosionSfx)
    {
        explosionSfx.GetComponent<ParticleSystem>().Play();
    }
}