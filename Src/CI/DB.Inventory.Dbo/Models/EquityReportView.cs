using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class EquityReportView
    {
        public int? BookId { get; set; }
        public string BookName { get; set; }
        public string Symbol { get; set; }
        public int? TradeDateInt { get; set; }
        public int? SettlmntDateInt { get; set; }
        public DateTime? TradeDate { get; set; }
        public DateTime? SettlmntDate { get; set; }
        public double? SideAdjustedQuantity { get; set; }
        public double? AveragePrice { get; set; }
        public string Currency { get; set; }
        public double? Spot { get; set; }
        public double? Amount { get; set; }
        public double? Commission { get; set; }
        public double? AmountWcommission { get; set; }
    }
}
