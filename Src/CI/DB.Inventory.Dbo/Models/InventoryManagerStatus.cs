using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InventoryManagerStatus
    {
        public DateTime BlotterImportTime { get; set; }
        public DateTime FxtransImportTime { get; set; }
    }
}
