using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using System.Diagnostics;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;
        }

        internal void Update()
        {
        }

        /// <summary>
        /// <para>角色进入地图</para>
        /// <para>通知该地图实例中其他玩家并把其他玩家列表发送给该玩家</para>
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character.Info);

            foreach (var kv in this.MapCharacters)
            {
                message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                this.SendCharacterEnterMap(kv.Value.connection, character.Info);
            }
            
            this.MapCharacters[character.entityId] = new MapCharacter(conn, character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        /// <summary>
        /// 通知一个客户端，一个玩家进入此地图实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="character"></param>
        void SendCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        internal void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, character.entityId);
            foreach (var ch in MapCharacters.Values)
            {
                Log.InfoFormat("SendToChar:{0}", ch.character.entityId);
                SendCharacterLeaveMap(ch.connection, character.Info);
            }
            this.MapCharacters.Remove(character.entityId);
        }

        void SendCharacterLeaveMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage
            {
                Response = new NetMessageResponse
                {
                    mapCharacterLeave = new MapCharacterLeaveResponse()
                    {
                        characterId = character.Entity.Id
                    }
                }
            };

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        internal void UpdateEntity(NEntity entity, EntityEvent entityEvent)
        {
            NEntitySync sync = new NEntitySync()
            {
                Id = entity.Id,
                Entity = entity,
                Event = entityEvent
            };
            foreach (var kv in MapCharacters)
            {
                if (kv.Key == entity.Id)
                {
                    kv.Value.character.EntityData = entity;
                }
                else
                {
                    SendEntitySync(kv.Value.connection, sync);
                }
            }
        }
        void SendEntitySync(NetConnection<NetSession> connection, NEntitySync sync)
        {
            NetMessage msg = new NetMessage()
            {
                Response = new NetMessageResponse()
                {
                    mapEntitySync = new MapEntitySyncResponse()
                }
            };
            msg.Response.mapEntitySync.entitySyncs.Add(sync);

            byte[] data = PackageHandler.PackMessage(msg);
            connection.SendData(data, 0, data.Length);
        }
    }
}
