using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class EntityManager : Singleton<EntityManager>
    {
        private int idx = 0;
        private List<Entity> allEntitys = new List<Entity>();
        private Dictionary<int, List<Entity>> mapEntitys = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapId, Entity entity)
        { 
            entity.EntityData.Id = ++idx;
            allEntitys.Add(entity);

            List<Entity> entities = null;
            if (!mapEntitys.TryGetValue(mapId, out entities))
            {
                mapEntitys[mapId] = new List<Entity>();
                entities = mapEntitys[mapId];
            }
            entities.Add(entity);
        }

        public void RemoveEntity(int mapId, Entity entity)
        {
            allEntitys.Remove(entity);
            mapEntitys[mapId].Remove(entity);
        }
    }
}
