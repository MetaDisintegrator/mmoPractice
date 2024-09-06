using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class FirstRequestService : Singleton<FirstRequestService>
    {
        public void Init()
        { }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstRequest>(OnFirstRequest);
        }

        private void OnFirstRequest(NetConnection<NetSession> sender, FirstRequest message)
        {
            Log.InfoFormat("OnFirstRequest[Str:{0}]", message.Str);
        }
    }
}
