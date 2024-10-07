using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class ItemManager
    {
        Character owner;
        Dictionary<int, Item> items = new Dictionary<int, Item>();

        public ItemManager(Character owner)
        {
            this.owner = owner;
            foreach (var item in owner.Data.Items)
            {
                items.Add(item.ItemID, new Item(item));
            }
        }

        public bool UseItem(int id, int count = 1)
        {
            if (items.TryGetValue(id, out var item))
            {
                if (item.Count >= count)
                {
                    //使用逻辑
                    item.Remove(count);
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(int id)
        {
            if (items.TryGetValue(id, out var item))
            {
                return item.Count > 0;
            }
            return false;
        }

        public void AddItem(int id, int count)
        {
            if (items.TryGetValue(id, out var item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = DBService.Instance.Entities.CharacterItem.Create();
                dbItem.CharacterID = owner.Id;
                dbItem.ItemID = id;
                dbItem.ItemCount = count;
                dbItem.Owner = owner.Data;
                owner.Data.Items.Add(dbItem);
                items.Add(id, new Item(dbItem));
            }
            Log.InfoFormat("Player {0} Add Item: [ID:{1},Count:{2}]", owner.entityId, id, count);
            DBService.Instance.Entities.SaveChanges();
        }

        public bool RemoveItem(int id, int count)
        {
            if (items.TryGetValue(id, out var item))
            {
                if (item.Count >= count)
                {
                    item.Remove(count); 
                    Log.InfoFormat("Player {0} Remove Item: [ID:{1},Count:{2}]",owner.entityId, id, count);
                    //DBService.Instance.Save();
                    return true;
                }
            }
            return false;
        }

        public void GetItemInfo(List<NItemInfo> buffer)
        {
            if (buffer.Count > 0)
            {
                Log.Warning("ItemManager: The buffer received already has items in it");
                buffer.Clear();
            }
                
            foreach (var item in items.Values)
            {
                buffer.Add(
                    new NItemInfo()
                    { 
                        Id = item.Id,
                        Count = item.Count,
                    });
            }
        }

        public Item GetItem(int id)
        { 
            if(items.TryGetValue(id,out var item))
                return item;
            return default;
        }

        internal void Clear()
        {
            foreach (var item in items.Values)
            {
                if (item.Count > 0)
                {
                    item.Remove(item.Count);
                }
            }
            Log.InfoFormat("Player {0} Remove All Items", owner.entityId);
        }
    }
}
