using Common;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class MapManager : Singleton<MapManager>
    {
        private Dictionary<int, Map> maps = new Dictionary<int, Map>();

        public MapManager() { }

        public Map this[int id]
        {
            get => maps[id];
        }

        public void Init()
        {
            foreach (var def in DataManager.Instance.Maps.Values)
            {
                maps[def.ID] = new Map(def);
                Log.InfoFormat("MapManagerInit>>ID:{0},Name:{1}", def.ID, def.Name);
            }
        }

        public void Update()
        {
            foreach (var map in maps.Values)
            {
                map.Update();
            }
        }
    }
}
