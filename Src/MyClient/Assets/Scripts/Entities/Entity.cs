using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Entity
    {
        public int entityId;

        public Vector3Int position;
        public Vector3Int direction;
        public int speed;

        private NEntity entityNetData;
        public NEntity EntityNetData
        { 
            get => entityNetData;
            set
            { 
                this.entityNetData = value;
                SetEntityData(value);
            }
        }

        public Entity(NEntity entityNetData)
        { 
            this.entityId = entityNetData.Id;
            this.EntityNetData = entityNetData;
        }

        public virtual void OnUpdate(float delta)
        {
            if (this.speed != 0)
            {
                Vector3 dir = this.direction;
                this.position += Vector3Int.RoundToInt(delta / 100f * speed * dir);
            }
            entityNetData.Position.FromVector3Int(this.position);
            entityNetData.Direction.FromVector3Int(this.direction);
            entityNetData.Speed = this.speed;
        }

        public void SetEntityData(NEntity entityNetData)
        {
            this.position =position.FromNVector3(entityNetData.Position);
            this.direction = direction.FromNVector3(entityNetData.Direction);
            this.speed = entityNetData.Speed;
        }
    }
}

