using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeSC.Data
{
    public class ItemModel
    {
        public Guid Id { get; set; } = new Guid();

        public String ItemName { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastTakenOut { get; set; }

        public User CreatedBy { get; set; }
    }
}
