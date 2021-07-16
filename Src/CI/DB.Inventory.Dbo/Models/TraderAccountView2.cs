using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderAccountView2
    {
        public string TraderId { get; set; }
        public int AccountId { get; set; }
        public int IsDefault { get; set; }
        public string Currency { get; set; }
        public string Shortname { get; set; }
    }
}
