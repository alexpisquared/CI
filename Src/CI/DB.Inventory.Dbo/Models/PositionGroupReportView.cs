using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class PositionGroupReportView
    {
        public int BookId { get; set; }
        public string CompanyName { get; set; }
        public string Symbol { get; set; }
        public string Usymbol { get; set; }
        public string ExchangeTicker { get; set; }
        public string Exchange { get; set; }
        public int? Position { get; set; }
        public double? AvgPx { get; set; }
        public string LongShort { get; set; }
    }
}
