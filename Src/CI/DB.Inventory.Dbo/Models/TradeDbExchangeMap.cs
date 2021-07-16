using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TradeDbExchangeMap
    {
        public string Exchange { get; set; }
        public string InventoryExchange { get; set; }
    }
}
