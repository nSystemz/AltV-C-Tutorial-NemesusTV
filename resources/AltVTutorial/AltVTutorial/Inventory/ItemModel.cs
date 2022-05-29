using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Inventory
{
    class ItemModel : IScript
    {
        public int id { get; set; }
        public string hash { get; set; }
        public string ownerEntity { get; set; }
        public int ownerIdentifier { get; set; }
        public int amount { get; set; }
        public Vector3 position { get; set; }

        public ItemModel Copy()
        {
            ItemModel itemModel = new ItemModel();
            itemModel.id = id;
            itemModel.hash = hash;
            itemModel.ownerEntity = ownerEntity;
            itemModel.ownerIdentifier = ownerIdentifier;
            itemModel.amount = amount;
            return itemModel;
        }

        public static ItemModel GetItemModelFromId(int itemId)
        {
            ItemModel item = null;
            foreach(ItemModel itemModel in Inventory.itemList)
            {
                if(itemModel.id == itemId)
                {
                    item = itemModel;
                    break;
                }
            }
            return item;
        }

        public static ItemModel GetClosestItemWithHash(TPlayer.TPlayer tplayer, string hash)
        {
            ItemModel item = null;
            foreach(ItemModel itemModel in Inventory.itemList)
            {
                if(itemModel.ownerEntity == "Ground" && item.hash == hash && tplayer.Position.Distance(item.position) < 2.0f)
                {
                    item = itemModel;
                    break;
                }
            }
            return item;
        }
    }
}
