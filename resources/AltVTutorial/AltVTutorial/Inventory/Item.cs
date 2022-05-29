using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Inventory
{
    class Item
    {
        public enum ItemTypes { Consumable, Useable }
        public string itemdescription { get; set; }
        public string hash { get; set; }
        public int type { get; set; }

        public Item(string itemdescription, string hash, int type)
        {
            this.itemdescription = itemdescription;
            this.hash = hash;
            this.type = type;
        }

        public static List<Item> ITEM_LIST = new List<Item>
        {
            new Item("Hot-Dog", "2565741261", (int)ItemTypes.Consumable),
        };

        public static string GetHashFromItem(string itemName)
        {
            string itemHash = null;
            foreach(Item item in ITEM_LIST)
            {
                if(item.itemdescription.ToLower() == itemName.ToLower())
                {
                    itemHash = item.hash;
                    break;
                }
            }
            return itemHash;
        }

        public static Item GetItemFromItemName(string itemName)
        {
            Item item = null;
            foreach(Item itemInList in ITEM_LIST)
            {
                if(itemInList.itemdescription == itemName)
                {
                    return itemInList;
                }
            }
            return item;
        }

        public static Item GetItemFromItemHash(string itemHash)
        {
            Item item = null;
            foreach(Item itemTemp in ITEM_LIST)
            {
                if(itemTemp.hash == itemHash)
                {
                    item = itemTemp;
                    break;
                }
            }
            return item;
        }
    }
}
