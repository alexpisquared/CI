using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CurrencySpreadView
    {
        public string CurrencyPair { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public double Spread { get; set; }
        public int CurrencySpreadGroupId { get; set; }
        public int CurrencySpreadId { get; set; }
        public string Groupname { get; set; }
        public string Shortname { get; set; }
    }
}
