using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeSC.Data
{
    public class InventoryLogModel
    {
        public Guid Id { get; set; }
        public String ItemName { get; set; }
        
        public int Quantity { get; set; }

        public String ApprovedBy { get; set; }

        public String TakenBy { get; set; }

        public DateTime DateTakenOut { get; set; }
    }
}
