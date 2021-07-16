using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CashTransPerBookTotalView
    {
        public int BookId { get; set; }
        public double? MiscCash { get; set; }
    }
}
