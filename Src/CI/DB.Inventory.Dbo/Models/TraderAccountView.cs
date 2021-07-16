using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderAccountView
    {
        public string TraderId { get; set; }
        public int AccountId { get; set; }
        public bool IsDefault { get; set; }
        public string Currency { get; set; }
    }
}
