using AltV.Net;
using AltV.Net.Resources.Chat.Api;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Inventory
{
    class Inventory : IScript
    {
        public static List<ItemModel> itemList;

        public static void RemoveItem(int id)
        {
            try
            {
                MySqlCommand command = Datenbank.Connection.CreateCommand();
                command.CommandText = "DELETE FROM items WHERE id = @id LIMIT 1";
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Utils.ConsoleLog("error", $"[RemoveItem]: " + e.ToString());
            }
        }

        public static List<ItemModel> LoadAllItems()
        {
            try
            {
                List<ItemModel> itemList = new List<ItemModel>();
                MySqlCommand command = Datenbank.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM items";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ItemModel item = new ItemModel();

                        float posX = reader.GetFloat("posX");
                        float posY = reader.GetFloat("posY");
                        float posZ = reader.GetFloat("posZ");

                        item.id = reader.GetInt32("id");
                        item.hash = reader.GetString("hash");
                        item.ownerEntity = reader.GetString("ownerEntity");
                        item.ownerIdentifier = reader.GetInt32("ownerIdentifier");
                        item.amount = reader.GetInt32("amount");
                        item.position = new System.Numerics.Vector3(posX, posY, posZ);

                        itemList.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Utils.ConsoleLog("error", $"[LoadAllItems]: " + e.ToString());
            }
            return itemList;
        }

        public static int AddNewItem(ItemModel item)
        {
            int itemId = 0;
            MySqlCommand mysqlCommand = Datenbank.Connection.CreateCommand();
            mysqlCommand.CommandText = "INSERT INTO items (hash, ownerEntity, ownerIdentifier, amount, posX, posY, posZ) VALUE (@hash, @ownerEntity, @ownerIdentifier, @amount, @posX, @posY, @posZ)";
            mysqlCommand.Parameters.AddWithValue("@hash", item.hash);
            mysqlCommand.Parameters.AddWithValue("@ownerEntity", item.ownerEntity);
            mysqlCommand.Parameters.AddWithValue("@ownerIdentifier", item.ownerIdentifier);
            mysqlCommand.Parameters.AddWithValue("@amount", item.amount);
            mysqlCommand.Parameters.AddWithValue("@posX", item.position.X);
            mysqlCommand.Parameters.AddWithValue("@posY", item.position.Y);
            mysqlCommand.Parameters.AddWithValue("@posZ", item.position.Z);

            mysqlCommand.ExecuteNonQuery();

            itemId = (int)mysqlCommand.LastInsertedId;

            return itemId;
        }

        public static void UpdateItem(ItemModel item)
        {
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "UPDATE items SET ownerEntity = @ownerEntity, ownerIdentifier = @ownerIdentifier, amount = @amount, posX = @posX, posY = @posY, posZ = @posZ WHERE id = @id LIMIT1";
            command.Parameters.AddWithValue("@ownerEntity", item.ownerEntity);
            command.Parameters.AddWithValue("@ownerIdentifier", item.ownerIdentifier);
            command.Parameters.AddWithValue("@amount", item.amount);
            command.Parameters.AddWithValue("@posX", item.position.X);
            command.Parameters.AddWithValue("@posY", item.position.Y);
            command.Parameters.AddWithValue("@posZ", item.position.Z);
            command.Parameters.AddWithValue("@id", item.id);

            command.ExecuteNonQuery();

        }

        private static List<InventoryModel> GetPlayerInventory(TPlayer.TPlayer tplayer)
        {
            List<InventoryModel> inventory = new List<InventoryModel>();

            foreach (ItemModel item in itemList.ToList())
            {
                if (item.ownerEntity == "Player" && item.ownerIdentifier == tplayer.SpielerID)
                {
                    InventoryModel inventoryItem = new InventoryModel();
                    Item getItem = Item.GetItemFromItemHash(item.hash);
                    inventoryItem.id = item.id;
                    inventoryItem.hash = item.hash;
                    inventoryItem.description = getItem.itemdescription;
                    inventoryItem.type = getItem.type;
                    inventoryItem.amount = item.amount;

                    inventory.Add(inventoryItem);
                }
            }
            return inventory;
        }

        public static void InventarAktionServer(TPlayer.TPlayer tplayer, int ItemId, string action)
        {
            List<InventoryModel> inventory = new List<InventoryModel>();
            inventory = GetPlayerInventory(tplayer);

            ItemModel item = ItemModel.GetItemModelFromId(ItemId);
            if (item == null) return;

            Item getItem = Item.GetItemFromItemHash(item.hash);

            switch (action.ToLower())
            {
                case "konsumieren":
                    {
                        if (getItem.type != (int)Item.ItemTypes.Consumable) return;
                        item.amount--;

                        Utils.sendNotification(tplayer, "success", $"Du konsumierst ein/e/en {getItem.itemdescription}!");
                        tplayer.SendChatMessage($"Du konsumierst ein/e/en {getItem.itemdescription}!");

                        if (item.amount <= 0)
                        {
                            RemoveItem(item.id);
                            itemList.Remove(item);
                        }
                        else
                        {
                            Inventory.UpdateItem(item);
                        }
                        break;
                    }
            }
        }

        public static ItemModel GetPlayerItemModelFromHash(int playerId, string hash)
        {
            ItemModel itemModel = null;
            foreach(ItemModel item in itemList)
            {
                if(item.ownerEntity == "Player" && item.ownerIdentifier == playerId && item.hash == hash)
                {
                    itemModel = item;
                    break;
                }
            }
            return itemModel;
        }
    }
}
