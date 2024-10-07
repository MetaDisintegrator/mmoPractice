using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBag : UIWindow
    {
        public Text txtMoney;
        public Transform[] pages;
        public GameObject bagItem;
        List<Image> slots;
        struct SlotItem
        { 
            public BagItem Item;
            public GameObject IconItem;

            public SlotItem(BagItem bagItem, GameObject go)
            {
                this.Item = bagItem;
                this.IconItem = go;
            }
        }
        Dictionary<int, SlotItem> slotItems;

        private void Start()
        {
            if (slots == null)
            {
                slots = new List<Image>();
                slotItems = new Dictionary<int, SlotItem>();
                foreach (var page in pages)
                {
                    slots.AddRange(page.GetComponentsInChildren<Image>());
                }
            }
            StartCoroutine(InitBag());
        }

        IEnumerator InitBag()
        {
            for (int i = 0; i < BagManager.Instance.Items.Length; i++)
            {
                BagItem item = BagManager.Instance.Items[i];
                if (item.ItemID > 0)
                {
                    PlaceItem(i, item);
                }
            }
            for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
            {
                slots[i].color = Color.gray;
            }
            yield return null;
        }

        public void OnReset()
        {
            BagManager.Instance.Reset();
            for (int i = 0; i < BagManager.Instance.Items.Length; i++)
            {
                BagItem item = BagManager.Instance.Items[i];
                if (item.ItemID > 0)
                {
                    if (slotItems.ContainsKey(i))
                    {
                        SlotItem slotItem = slotItems[i];
                        if (slotItem.Item != item)
                        {
                            if (slotItem.Item.ItemID != item.ItemID)
                            {
                                Destroy(slotItem.IconItem);
                                PlaceItem(i, item);
                            }
                            else
                            { 
                                slotItem.IconItem.GetComponent<UIIconItem>().SetCount(item.Count);
                                slotItem.Item.Count = item.Count;
                                slotItems[i] = slotItem;
                            }
                        }
                    }
                    else
                    {
                        PlaceItem(i, item);
                    }
                }
            }
            for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
            {
                slots[i].color = Color.gray;
            }
        }

        private void PlaceItem(int slotIndex, BagItem item)
        {
            GameObject go = Instantiate(bagItem, slots[slotIndex].transform);
            UIIconItem icon = go.GetComponent<UIIconItem>();
            ItemDefine def = DataManager.Instance.Items[item.ItemID];
            icon.SetMainIcon(def.Icon, item.Count);
            slotItems[slotIndex] = new SlotItem(item, go);
        }
    }
}