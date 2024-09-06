using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum NpcType
    { 
        None = 0,
        Task = 1,
        Function = 2,
    }
    public enum NpcFunction
    { 
        None = 0,
        InvokeInsrance =1,
        InvokeShop = 2,
    }
    public class NpcDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public NpcType Type { get; set; }
        public NpcFunction Function { get; set; }
        public int Param { get; set; }
    }
}
