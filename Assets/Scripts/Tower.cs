using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Tower : Entity
{
    [SerializeField] private Weapon projectile;
    private Coroutine _currentCoroutine;
    [SerializeField] private bool isMainTower;
    private Entity _attackTarget;
    private float _attackTargetColliderOffset;
    private Vector3 _attackTargetPos;


    public override float Health
    {
        get => health;
        set
        {
            HealthTween.Kill();
            healthBarCanvas.healthBarSlider.gameObject.SetActive(true);
            HealthTween = healthBarCanvas.healthBarSlider.DOValue(health / entityClassType.maxHealth, 1);
            health = value > entityClassType.maxHealth ? entityClassType.maxHealth : value;
            if (health < entityClassType.maxHealth && currentBehaviour == TowerBehaviour.Idle)
            {
                UpdateTowerState(TowerBehaviour.FindingClosestTarget);
            }
        }
    }

    public enum TowerBehaviour
    {
        Idle,
        FindingClosestTarget,
        Acting,
        Dying
    }

    public static event Action<TowerBehaviour> OnTowerStateChanged;
    public TowerBehaviour currentBehaviour;

    private void Start()
    {
        UpdateTowerState(TowerBehaviour.Idle);
        healthBarCanvas.transform.rotation =
            Quaternion.LookRotation(new Vector3(transform.position.x, CameraPos.y, CameraPos.z) - transform.position);
    }

    protected override void LateUpdate()
    {
    }

    public void UpdateTowerState(TowerBehaviour newState)
    {
        currentBehaviour = newState;

        if (newState == TowerBehaviour.Idle)
        {
            if (!isMainTower)
            {
                UpdateTowerState(TowerBehaviour.FindingClosestTarget);
            }
        }

        if (newState == TowerBehaviour.FindingClosestTarget)
        {
            _currentCoroutine = StartCoroutine(FindingTarget());
        }

        if (newState == TowerBehaviour.Acting)
        {
            _currentCoroutine = StartCoroutine(Acting());
        }

        if (newState == TowerBehaviour.Dying)
        {
            gameObject.SetActive(false);
            if (isMainTower) return;

            if (isAlly)
            {
                EntityManager.Instance.enemyMainTower.UpdateTowerState(TowerBehaviour.FindingClosestTarget);
            }
            else
            {
                EntityManager.Instance.allyMainTower.UpdateTowerState(TowerBehaviour.FindingClosestTarget);
            }
        }

        OnTowerStateChanged?.Invoke(newState);
    }

    private IEnumerator FindingTarget()
    {
        while (currentBehaviour == TowerBehaviour.FindingClosestTarget)
        {
            FindClosestTarget();
            if (FindClosestTarget() - target.ColliderOffset() <= entityClassType.rangeRadius)
            {
                UpdateTowerState(TowerBehaviour.Acting);
            }

            yield return null;
        }
    }

    private IEnumerator Acting()
    {
        _attackTarget = target;
        _attackTargetColliderOffset = target.ColliderOffset();
        _attackTargetPos = target.transform.position;
        var direction =
            _attackTargetPos - transform.position + new Vector3(0, 10, 0); // y değeri offset
        var arrow = PoolManager.Instance.arrowPool.Pull(transform.position + new Vector3(0, 5, 0));
        arrow.owner = this;
        arrow.transform.LookAt(direction);
        var time = FindClosestTarget() / 20; // 20 is speedper pixel
        var _targetsPos = _attackTargetPos;
        arrow.transform.DOMove(_targetsPos + new Vector3(0, 1, 0), time).OnComplete(() =>
        {
            if (!_attackTarget) return;
            arrow.OnHit(_attackTarget);
        });
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget() - _attackTargetColliderOffset > entityClassType.rangeRadius)
        {
            UpdateTowerState(TowerBehaviour.FindingClosestTarget);
        }
        else
        {
            _currentCoroutine = StartCoroutine(Acting());
        }

        yield return null;
    }

    protected override void DeathSequence()
    {
        UpdateTowerState(TowerBehaviour.Dying);
    }
}