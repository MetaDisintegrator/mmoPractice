using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class MapService : Singleton<MapService>
    {
        public MapService() 
        {
            
        }

        public void Init()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(OnMapEntitySyncRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(OnMapTeleportRequest);
        }

        private void OnMapEntitySyncRequest(NetConnection<NetSession> sender, MapEntitySyncRequest message)
        {
            Character character = sender.Session.Character;
            NEntitySync sync = message.entitySync;
            //Log.InfoFormat("MapEntitySyncRequest[Id:{0},MapId:{1},Entity:{2},Event:{3}]",sync.Id,character.Info.mapId,sync.Entity.String(),sync.Event);
            Log.Info("MapEntitySyncRequest");
            MapManager.Instance[character.Info.mapId].UpdateEntity(sync.Entity, sync.Event);
        }

        private void OnMapTeleportRequest(NetConnection<NetSession> sender, MapTeleportRequest message)
        {
            Log.InfoFormat("MapTeleportRequest[Id:{0}]", message.teleporterId);
            if (!DataManager.Instance.Teleporters.ContainsKey(message.teleporterId))
            {
                Log.ErrorFormat("Teleporter: [{0}] not exist", message.teleporterId);
                return;
            }
            TeleporterDefine sourceDef = DataManager.Instance.Teleporters[message.teleporterId];
            if (sourceDef.LinkTo < 0 || !DataManager.Instance.Teleporters.ContainsKey(sourceDef.LinkTo))
            {
                Log.ErrorFormat("Teleporter: [{0}] has no valid link", message.teleporterId);
                return;
            }
            TeleporterDefine targetDef = DataManager.Instance.Teleporters[sourceDef.LinkTo];

            Character character = sender.Session.Character;
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
            character.Position = targetDef.Position;
            character.Direction = targetDef.Direction;
            MapManager.Instance[targetDef.MapID].CharacterEnter(sender, character);
        }
    }
}
