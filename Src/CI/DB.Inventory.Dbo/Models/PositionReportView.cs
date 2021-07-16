using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class PositionReportView
    {
        public string Usymbol { get; set; }
        public string Symbol { get; set; }
        public string SecurityType { get; set; }
        public string ExchangeTicker { get; set; }
        public string Exchange { get; set; }
        public string CompanyName { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime UpdateDate { get; set; }
        public double ProfitLoss { get; set; }
        public int QuantityOutstanding { get; set; }
        public double AveragePrice { get; set; }
        public double TotalCommissions { get; set; }
        public string LongShort { get; set; }
    }
}
