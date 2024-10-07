using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class BagService : Singleton<BagService>
    {
        public BagService()
        {
        }

        public void Init()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(OnBagSave);
        }

        private void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest message)
        {
            if(message.BagInfo ==  null)
                return;
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest[characterID:{0}, Unlocked:{1}]", character.entityId, message.BagInfo.Unlocked);
            character.Info.Bag = message.BagInfo;
            DBService.Instance.Save();
        }
    }
}
