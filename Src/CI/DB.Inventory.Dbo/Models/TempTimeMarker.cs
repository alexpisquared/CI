using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TempTimeMarker
    {
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
