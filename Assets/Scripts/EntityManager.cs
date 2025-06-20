using System.Collections;
using System.Collections.Generic;
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
}
