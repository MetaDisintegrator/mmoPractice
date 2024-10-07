using Models;
using SkillBridge.Message;
using UnityEngine;

namespace Managers
{
    public class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;
        NBagInfo info;
        unsafe public void Init(NBagInfo info)
        {
            this.info = info;
            this.Unlocked = info.Unlocked;
            this.Items = new BagItem[info.Unlocked];
            Debug.LogFormat("ItemBytes Length: {0}",info.Items.Length);
            if (info.Items != null && info.Items.Length >= info.Unlocked * sizeof(BagItem))
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[info.Unlocked * sizeof(BagItem)];
                Reset();
            }
        }
        public void Reset()
        {
            int i = 0;
            foreach (var item in ItemManager.Instance.items.Values)
            {
                int limit = item.Define.MaxStack, count = item.Count;
                while (count > limit)
                {
                    Items[i].ItemID = (ushort)item.Id;
                    Items[i].Count = (ushort)limit;
                    i++;
                    count -= limit;
                }
                if (count > 0)
                {
                    Items[i].ItemID = (ushort)item.Id;
                    Items[i].Count = (ushort)count;
                    i++;
                }
            }
        }

        unsafe void Analyze(byte[] bytes)
        {
            fixed (byte* p = bytes)
            {
                for (int i = 0; i < Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(p + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }
        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* p = info.Items)
            {
                for (int i = 0; i < Unlocked; i++)
                { 
                    BagItem *item = (BagItem*)(p + i * sizeof (BagItem));
                    *item = Items[i];
                }
            }
            return info;
        }
    }
}