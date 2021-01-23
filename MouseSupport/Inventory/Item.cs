using System;
using System.Collections.Generic;
using UnityEngine;

namespace MouseSupport.Inventory
{
    public class Item : IComparable<Item>
    {
        public readonly int id;
        public int count;
        public readonly int maxcount;
        public readonly int price;
        public readonly int heal;
        public readonly Sprite sprite;

        public Item(int id, int count)
        {
            this.id = id;
            this.count = count;
            this.maxcount = DB.ITEM_DEFS[id].hold_limit;
            this.price = DB.ITEM_DEFS[id].price;
            this.sprite = PT2.sprite_lib.all_item_sprites[DB.ITEM_DEFS[id].graphic_id];
            this.heal = 0;

            try
            {
                string effects = DB.ITEM_DEFS[id].Get_EFFECTs();
                if (effects != null)
                {
                    string[] strArray = effects.Split(',');
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        if (strArray[index] == "hp" && (index + 1 < strArray.Length))
                        {
                            this.heal = int.Parse(strArray[index + 1]);
                        }
                    }
                }
            }
            catch(Exception)
            {
                this.heal = 0;
            }
        }

        public int CompareTo(Item other)
        {
            if (other == null)
                return 1;

            return price.CompareTo(other.price);
        }

        public class PriceComparer : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                return x.price.CompareTo(y.price);
            }
        }

        public class HealComparer : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                return x.heal.CompareTo(y.heal);
            }
        }
    }
}
