using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CusipFix
    {
        public int RecNo { get; set; }
        public string Symbol { get; set; }
        public string ExchangeTicker { get; set; }
        public string Exchange { get; set; }
        public string Cusip { get; set; }
    }
}
