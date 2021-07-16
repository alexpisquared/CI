using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InvTransactionConsolidatedView
    {
        public string BaseSymbol { get; set; }
        public int Side { get; set; }
        public double? Quantity { get; set; }
        public double? SideAdjustedQuantity { get; set; }
        public double? AvgPx { get; set; }
        public int? SettlmntDateInt { get; set; }
    }
}
