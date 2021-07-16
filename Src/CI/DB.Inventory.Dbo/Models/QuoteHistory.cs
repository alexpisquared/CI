using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class QuoteHistory
    {
        public string Symbol { get; set; }
        public int DateInt { get; set; }
        public string ExchangeCode { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? Change { get; set; }
        public decimal? Vol { get; set; }
        public decimal? Trades { get; set; }
    }
}
