using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Etf
    {
        public int ImportDate { get; set; }
        public string Symbol { get; set; }
        public string Country { get; set; }
        public int Free { get; set; }
    }
}
