using Common.Data;
using Entities;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public interface IEntityNotify
    { 
        void OnEntityRemoved();
        void OnEntityChanged(NEntity entity);
        void OnEntityEvent(EntityEvent entityEvent);
    }

    public class EntityManager : Singleton<EntityManager>, IDisposable
    {
        private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        private Dictionary<int, IEntityNotify> entityNotifiers = new Dictionary<int, IEntityNotify>();

        public void Init()
        {
        }

        public void Dispose()
        {
        }

        public void RegisterEntityNotifier(int entityId, IEntityNotify notifier) 
        {
            entityNotifiers[entityId] = notifier;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(int entityId)
        {
            entities.Remove(entityId);
            if (entityNotifiers.ContainsKey(entityId))
            {
                entityNotifiers[entityId]?.OnEntityRemoved();
                entityNotifiers.Remove(entityId);
            }
        }

        public void Clear()
        {
        }

        public void UpdateEntity(NEntitySync sync)
        {
            if (!entities.TryGetValue(sync.Id, out Entity entity))
                return;
            if (entity != null)
            {
                if(sync.Entity != null)
                entity.SetEntityData(sync.Entity);
                if (entityNotifiers.ContainsKey(sync.Id))
                {
                    entityNotifiers[sync.Id]?.OnEntityChanged(sync.Entity);
                    entityNotifiers[sync.Id]?.OnEntityEvent(sync.Event);
                }
            }
        }
    }
}

