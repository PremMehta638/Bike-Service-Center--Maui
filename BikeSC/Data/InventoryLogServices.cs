using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikeSC.Data
{
    public class InventoryLogServices
    {
        private static void SaveItem(List<InventoryLogModel> items)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string GetAppInventoryFilePath = Utils.GetAppInventoryLogFilePath();
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(GetAppInventoryFilePath, json);
        }

        public static List<InventoryLogModel> WriteInventoryLogData(Guid id, String itemName, int quantity, String approvedBy, String takenBy, DateTime dateTakenOut)
        {
            List<InventoryLogModel> Items = ReadInventoryLogData();
            Items.Add(
                new InventoryLogModel
                {
                    Id = id,
                    ItemName = itemName,    
                    Quantity = quantity,    
                    ApprovedBy = approvedBy,    
                    TakenBy = takenBy,
                    DateTakenOut = dateTakenOut,
                }
            );
            SaveItem(Items);
            return Items;
        }

        public static List<InventoryLogModel> ReadInventoryLogData()
        {
            string appItemfilepath = Utils.GetAppInventoryLogFilePath();

            if (!File.Exists(appItemfilepath))
            {
                return new List<InventoryLogModel>();
            }
            var json = File.ReadAllText(appItemfilepath);
            return JsonSerializer.Deserialize<List<InventoryLogModel>>(json);
        }

        public static List<InventoryLogModel> Delete(Guid id)
        {
            List<InventoryLogModel> Items = ReadInventoryLogData();
            InventoryLogModel Item = Items.FirstOrDefault(x => x.Id == id);

            if (Item == null)
            {
                throw new Exception("Item not found.");
            }

            Items.Remove(Item);
            SaveItem(Items);
            return Items;
        }
    }
}
