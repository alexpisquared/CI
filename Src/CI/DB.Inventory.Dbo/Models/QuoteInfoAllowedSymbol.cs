using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class QuoteInfoAllowedSymbol
    {
        public string Symbolstring { get; set; }
        public string Exchange { get; set; }
        public string Description { get; set; }
        public string IssueType { get; set; }
        public string Cusip { get; set; }
        public bool? OptionAvailable { get; set; }
        public string ExDivDate { get; set; }
        public double? Dividend { get; set; }
        public string DivRate { get; set; }
        public double? Yield { get; set; }
        public string ListingType { get; set; }
    }
}
