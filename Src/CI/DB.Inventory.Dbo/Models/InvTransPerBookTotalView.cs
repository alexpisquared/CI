using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InvTransPerBookTotalView
    {
        public int? BookId { get; set; }
        public double? Commission { get; set; }
        public double? Equity { get; set; }
    }
}
