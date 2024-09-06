using Common;
using GameServer.Entities;
using GameServer.Models;
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

        public bool UseItem(int id, int count)
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
                TCharacterItem characterItem = new TCharacterItem()
                {
                    Id = id,
                    CharacterID = owner.Id,
                    ItemID = id,
                    ItemCount = count,
                    Owner = owner.Data
                };
                items.Add(id, new Item(characterItem));
            }
        }

        public bool RemoveItem(int id, int count)
        {
            if (items.TryGetValue(id, out var item))
            {
                if (item.Count >= count)
                {
                    item.Remove(count); 
                    return true;
                }
            }
            return false;
        }

        public void GetItemInfo(List<Item> buffer)
        {
            if (buffer.Count > 0)
            {
                Log.Warning("ItemManager: The buffer received already has items in it");
                buffer.Clear();
            }
                
            foreach (var item in items.Values)
            {
                buffer.Add(item);
            }
        }
    }
}
