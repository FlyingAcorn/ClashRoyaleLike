using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tower : Entity
{
    [SerializeField] private Weapon projectile;
    private Coroutine _currentCoroutine;

    public enum TowerBehaviour
    {
        FindingClosestTarget,
        Acting,
        Dying
    }

    public static event Action<TowerBehaviour> OnTowerStateChanged;
    public TowerBehaviour currentBehaviour;

    private void Start()
    {
        UpdateTowerState(TowerBehaviour.FindingClosestTarget);
    }

    public void UpdateTowerState(TowerBehaviour newState)
    {
        currentBehaviour = newState;

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
        }

        OnTowerStateChanged?.Invoke(newState);
    }

    /* find the closest target in range
    act*/
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
        var direction = target.transform.position - transform.position + new Vector3(0, 10, 0); // y değeri offset
        var arrow = Instantiate(projectile, transform.position + new Vector3(0, 5, 0),
            Quaternion.LookRotation(direction));
        arrow.owner = this;
        var time = FindClosestTarget() / 20; // 20 is speedper pixel
        var _targetsPos = target.transform.position;
         var tween =arrow.transform.DOMove(_targetsPos + new Vector3(0, 1, 0), time).OnComplete(() =>
            DOVirtual.DelayedCall(3,(() => Destroy(arrow))));;
        arrow.GetComponent<Arrow>().CurrentTween = tween;
        yield return new WaitForSeconds(entityClassType.attackSpeed);
        if (FindClosestTarget() - target.ColliderOffset() > entityClassType.rangeRadius)
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
