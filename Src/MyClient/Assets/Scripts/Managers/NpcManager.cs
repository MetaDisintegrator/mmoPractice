using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class NpcManager : Singleton<NpcManager>
    {
        public delegate bool NpcActionHandler(NpcDefine define);

        Dictionary<NpcFunction, NpcActionHandler> funcs = new Dictionary<NpcFunction, NpcActionHandler>();

        public void RegisterNpcAction(NpcFunction function, NpcActionHandler handler)
        {
            if (funcs.ContainsKey(function))
                funcs[function] += handler;
            else
                funcs[function] = handler;
        }

        public NpcDefine GetDefine(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
                return DataManager.Instance.Npcs[npcId];
            return default;
        }

        public bool Interact(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
            { 
                NpcDefine npc = DataManager.Instance.Npcs[npcId];
                return Interact(npc);
            }
            return false;
        }
        private bool Interact(NpcDefine npc)
        {
            if (npc.Type == NpcType.Task)
                return TaskInteract(npc);
            else if (npc.Type == NpcType.Function)
                return FuncInteract(npc);
            return false;
        }

        private bool TaskInteract(NpcDefine npc)
        {
            MessageBox.Show("任务对话：" + npc.Name);
            return true;
        }
        private bool FuncInteract(NpcDefine npc)
        {
            if (npc.Type != NpcType.Function)
                return false;
            if (funcs.ContainsKey(npc.Function) && funcs[npc.Function] != null)
                return funcs[npc.Function].Invoke(npc);
            return false;
        }
    }
}

