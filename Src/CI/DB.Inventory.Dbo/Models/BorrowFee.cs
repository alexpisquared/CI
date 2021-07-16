using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BorrowFee
    {
        public int AccountId { get; set; }
        public double MinRate { get; set; }
        public double MinSpread { get; set; }
        public double Spread { get; set; }
    }
}
