using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CompanyProfileCusipMatch
    {
        public string Cusip { get; set; }
        public int? MatchCount { get; set; }
    }
}
