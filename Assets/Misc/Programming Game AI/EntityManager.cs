using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kpable.Utilities;

namespace Kpable.AI.FSM
{
    public class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<int, BaseGameEntity> entities = new Dictionary<int, BaseGameEntity>();

        public void RegisterEntity(BaseGameEntity entity)
        {
            entities.Add(entity.ID, entity);
        }

        public void RemoveEntity(BaseGameEntity entity)
        {
            entities.Remove(entity.ID);
        }

        public BaseGameEntity GetEntityFromId(int id)
        {
            BaseGameEntity entityToReturn;
            if (entities.TryGetValue(id, out entityToReturn))
                return entityToReturn;
            return null;
        }
    }
}