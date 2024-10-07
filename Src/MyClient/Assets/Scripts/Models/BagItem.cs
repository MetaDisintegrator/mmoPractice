using System;
using System.Runtime.InteropServices;

namespace Models
{
    [StructLayout(LayoutKind.Sequential,Pack =1)]
    public struct BagItem
    {
        public ushort ItemID;
        public ushort Count;

        public static BagItem zero = new BagItem(0, 0);

        public BagItem(ushort id, ushort count)
        {
            this.ItemID = id;
            this.Count = count;
        }

        public static bool operator ==(BagItem l, BagItem r)
        {
            return l.ItemID == r.ItemID && l.Count == r.Count;
        }
        public static bool operator !=(BagItem l, BagItem r)
        {
            return l.ItemID != r.ItemID && l.Count == r.Count;
        }

        public override bool Equals(object obj)
        {
            return obj is BagItem item &&
                   ItemID == item.ItemID &&
                   Count == item.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemID, Count);
        }
    }
}