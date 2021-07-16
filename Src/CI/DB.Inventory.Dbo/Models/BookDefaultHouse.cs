using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BookDefaultHouse
    {
        public int LeadId { get; set; }
        public int HouseId { get; set; }

        public virtual Book Lead { get; set; }
    }
}
