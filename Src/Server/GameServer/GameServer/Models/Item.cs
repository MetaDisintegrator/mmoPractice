using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    internal class Item
    {
        TCharacterItem dbItem;

        public int Id { get; set; }
        public int Count { get; set; }

        public Item(TCharacterItem item) 
        {
            this.dbItem = item;
            this.Id = item.Id;
            this.Count = item.ItemCount;
        }

        public void Add(int count)
        {
            Count += count;
            dbItem.ItemCount += count;
        }
        public void Remove(int count)
        {
            Count -= count;
            dbItem.ItemCount -= count;
        }

        public bool Use()
        {
            return false;
        }
        public override string ToString()
        {
            return string.Format("Item(id:{0},count:{1})", Id, Count);
        }
    }
}
