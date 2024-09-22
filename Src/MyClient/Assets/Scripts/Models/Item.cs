using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class Item
    {
        public int Id;
        public int Count;

        public Item(NItemInfo itemInfo)
        { 
            Id = itemInfo.Id;
            Count = itemInfo.Count;
        }

        public override string ToString()
        {
            return string.Format("Item:[Id:{0},Count:{1}]", Id, Count);
        }
    }
}
