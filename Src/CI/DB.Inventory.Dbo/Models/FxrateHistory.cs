using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class FxrateHistory
    {
        public string Currency { get; set; }
        public int DateInt { get; set; }
        public DateTime UpdateTime { get; set; }
        public double Rate { get; set; }
    }
}
