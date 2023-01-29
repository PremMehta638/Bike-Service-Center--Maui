using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikeSC.Data
{
    public class ItemServices
    {
        private static void SaveItemData(List<ItemModel> items)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string GetAppStudentsFilePath = Utils.GetAppItemsFilePath();
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(GetAppStudentsFilePath, json);
        }

        public static List<ItemModel> CreateNewItem(Guid id, String itemName, int quantity, DateTime createdAt, User createdBy)
        {


            List<ItemModel> Items = ReadItemData();
            Items.Add(
                new ItemModel
                {
                    Id = id,    
                    ItemName = itemName,
                    Quantity = quantity,
                    CreatedAt = createdAt,  
                    CreatedBy = createdBy,
                }
            );
            SaveItemData(Items);
            return Items;
        }

        public static List<ItemModel> ReadItemData()
        {
            string appItemFilepath = Utils.GetAppItemsFilePath();

            if (!File.Exists(appItemFilepath))
            {
                return new List<ItemModel>();
            }
            var json = File.ReadAllText(appItemFilepath);
            return JsonSerializer.Deserialize<List<ItemModel>>(json);
        }

        public static List<ItemModel> UpdateItemData(Guid id, string itemName, int quantity, DateTime? LastTakenOut = null)
        {
            List<ItemModel> items = ReadItemData();
            ItemModel ItemUpdate = items.FirstOrDefault(x => x.Id == id); 

            if (ItemUpdate == null)
            {
                throw new Exception("Items not found.");
            }

            ItemUpdate.ItemName = itemName;
            ItemUpdate.Quantity = quantity;
            ItemUpdate.LastTakenOut= LastTakenOut;
            SaveItemData(items);
            return items;
        }

        public static List<ItemModel> DeleteItemData(Guid id)
        {
            List<ItemModel> Items = ReadItemData();
            ItemModel Item = Items.FirstOrDefault(x => x.Id == id);

            if (Item == null)
            {
                throw new Exception("Items not found.");
            }

            Items.Remove(Item);
            SaveItemData(Items);
            return Items;
        }

    }
}
