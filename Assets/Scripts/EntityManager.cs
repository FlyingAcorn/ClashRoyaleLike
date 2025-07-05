using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class EntityManager : Singleton<EntityManager>
{
    public List<Entity> allies;
    public List<Entity> enemies;

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
    }

    public void InitialSort() // state play olmadan once yapmak lazım yoksa 5 ten sonra empty alıyor.
    {
        var list = GetComponentsInChildren<Entity>().ToList();
        allies.AddRange(list.Select(c => c).Where(c => c.isAlly));
        enemies.AddRange(list.Select(c => c).Where(c => !c.isAlly));
    }
}