using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class EntityManager : Singleton<EntityManager>
{
    public List<Entity> allies;
    public List<Entity> enemies;
    [SerializeField] public GameObject entitiesOnMap;
    [SerializeField] private GameObject pooledObj;
    public Tower allyMainTower;
    public Tower enemyMainTower;
    public List<Arrow> arrowPool;
    public List<FireBall> fireballPool;
    public List<ExplosionSfx> explosionSfxPool;


    public void AddEntity(Entity entity)
    {
        if (entity.isAlly)
        {
            allies.Add(entity);
        }
        else
        {
            enemies.Add(entity);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitialSort();
        FillPools(arrowPool, 30);
        FillPools(fireballPool, 10);
        FillPools(explosionSfxPool, 10);
    }

    public void InitialSort() // state play olmadan once yapmak lazım yoksa 5 ten sonra empty alıyor.
    {
        var list = entitiesOnMap.GetComponentsInChildren<Entity>().ToList();
        allies.AddRange(list.Select(c => c).Where(c => c.isAlly));
        enemies.AddRange(list.Select(c => c).Where(c => !c.isAlly));
    }

    private void FillPools<T>(List<T> list, int duplication) where T : Object
    {
        var firstObj = list[0];
        for (int i = 0; i < duplication; i++)
        {
            var duplicate = Instantiate(firstObj, pooledObj.transform);
            list.Add(duplicate);
        }

        list.Remove(firstObj);
    }
}