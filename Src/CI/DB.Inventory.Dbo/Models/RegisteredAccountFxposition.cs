using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class RegisteredAccountFxposition
    {
        public int TradeDateInt { get; set; }
        public string PensonAccountId { get; set; }
        public double Usdamount { get; set; }
    }
}
