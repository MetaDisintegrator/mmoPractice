using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;

        public Item(NItemInfo itemInfo)
        { 
            Id = itemInfo.Id;
            Count = itemInfo.Count;
            Define = DataManager.Instance.Items[Id];
        }

        public override string ToString()
        {
            return string.Format("Item:[Id:{0},Count:{1}]", Id, Count);
        }
    }
}
