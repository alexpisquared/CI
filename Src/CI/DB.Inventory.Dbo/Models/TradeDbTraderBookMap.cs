using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TradeDbTraderBookMap
    {
        public string TraderId { get; set; }
        public string BookShortNamePrefix { get; set; }
    }
}
