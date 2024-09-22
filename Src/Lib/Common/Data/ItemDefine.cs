﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum ItemFunction
    {
        None = 0,
        RecoverHP = 1,
        RecoverMp = 2,
        AddBuff = 3,
        AddExp = 4,
        AddMoney = 5,
        AddItem = 6,
        AddSkillPoint = 7,
    }

    public class ItemDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public string Category { get; set; }
        public bool CanUse { get; set; }
        public float UseCd { get; set; }
        public int Prise { get; set; }
        public int SellPrise {  get; set; }
        public int MaxStack { get; set; }
        public string Icon { get; set; }
        public ItemFunction Function { get; set; }
        public int Param { get; set; }
        public List<int> Params { get; set; }
    }
}
