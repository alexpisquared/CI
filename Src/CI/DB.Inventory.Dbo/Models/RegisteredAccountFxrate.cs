using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class RegisteredAccountFxrate
    {
        public int TradeDateInt { get; set; }
        public string Currency { get; set; }
        public double? BuyRate { get; set; }
        public double? SellRate { get; set; }
    }
}
