using Models;
using SkillBridge.Message;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        Dictionary<int, Item> items = new Dictionary<int, Item>();
        public void Init(List<NItemInfo> infos)
        { 
            this.items.Clear();
            Item item;
            foreach (NItemInfo info in infos)
            {
                item = new Item(info);
                items.Add(info.Id, item);
                Debug.LogFormat("ItemManager Init: {0}", item);
            }
        }
    }
}