using Common.Data;
using Entities;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Events;

namespace Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {
        public int currentMapId = -1;

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(OnMapEntitySync);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
        {
            Debug.LogFormat("OnMapCharacterLeave:{0}",message.characterId);
            if (message.characterId == User.Instance.CurrentCharacter.Entity.Id)
            {
                CharacterManager.Instance.Clear();
            }
            else
            { 
                CharacterManager.Instance.RemoveCharacter(message.characterId);
            }
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter:[MapId:{0} Count:{1}]", message.mapId, message.Characters.Count);
            if (message.mapId != currentMapId)
            {
                SceneManager.Instance.onComplete += () => CharacterEnter(message.Characters);
                EnterMap(message.mapId);
                currentMapId = message.mapId;
            }
            else
                CharacterEnter(message.Characters);
        }
        private void CharacterEnter(List<NCharacterInfo> characters)
        {
            foreach (var character in characters)
            {
                if (character.Id == User.Instance.CurrentCharacter.Id)//当前角色切换地图，做更新
                    User.Instance.CurrentCharacter = character;
                CharacterManager.Instance.AddCharacter(character);
            }
        }
        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine def = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = def;
                SceneManager.Instance.LoadScene(def.Resource);
            }
            else
                Debug.LogErrorFormat("EnterMap:[id:{0}] not exist");
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            //Debug.LogFormat("MapEntitySync[Id:{0},entity:{1},event:{2}]", entity.Id, entity.String(), entityEvent.ToString("G"));
            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest()
                {
                    mapEntitySync = new MapEntitySyncRequest()
                    {
                        entitySync = new NEntitySync()
                        {
                            Id = entity.Id,
                            Entity = entity,
                            Event = entityEvent
                        }
                    }
                }
            };

            NetClient.Instance.SendMessage(msg);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse message)
        {
            StringBuilder builder = new StringBuilder($"OnMapEntitySync:[Count:{message.entitySyncs.Count}]");
            builder.AppendLine();
            foreach (var sync in message.entitySyncs)
            { 
                builder.AppendLine($"[Id:{sync.Id},Entity:{sync.Entity.String()},Event:{sync.Event}]");
                EntityManager.Instance.UpdateEntity(sync);
            }
            Debug.Log(builder.ToString());
        }

        public void SendMapTeleport(int teleporterId)
        {
            Debug.LogFormat("MapTeleport[teleporterId:{0}]", teleporterId);
            NetMessage msg = new NetMessage()
            {
                Request = new NetMessageRequest()
                {
                    mapTeleport = new MapTeleportRequest()
                    {
                        teleporterId = teleporterId,
                    }
                }
            };
            NetClient.Instance.SendMessage(msg);
        }
    }
}

