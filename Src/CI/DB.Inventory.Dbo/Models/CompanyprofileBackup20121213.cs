using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CompanyprofileBackup20121213
    {
        public string Usymbol { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Currency { get; set; }
        public string SecurityType { get; set; }
        public string ExchangeTicker { get; set; }
        public string Exchange { get; set; }
        public string BaseSymbol { get; set; }
        public string Cusip { get; set; }
        public string Isin { get; set; }
        public double Multiplier { get; set; }
    }
}
