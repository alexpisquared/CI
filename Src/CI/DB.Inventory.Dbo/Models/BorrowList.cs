using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BorrowList
    {
        public string Location { get; set; }
        public string Account { get; set; }
        public int? AmountAvailable { get; set; }
        public string Symbol { get; set; }
        public int? StartDate { get; set; }
        public int? ExpiryDate { get; set; }
        public int FileDateInt { get; set; }
        public int? AmountUsed { get; set; }
        public string Source { get; set; }
        public string AddBy { get; set; }
    }
}
