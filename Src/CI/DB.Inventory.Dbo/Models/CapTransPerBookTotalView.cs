using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CapTransPerBookTotalView
    {
        public int BookId { get; set; }
        public double? CapCash { get; set; }
    }
}
