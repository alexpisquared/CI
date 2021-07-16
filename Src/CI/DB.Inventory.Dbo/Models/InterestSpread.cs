using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InterestSpread
    {
        public string InterestCode { get; set; }
        public string UnderlyingInterestCode { get; set; }
        public string Currency { get; set; }
        public double? RangeStart { get; set; }
        public double RangeEnd { get; set; }
        public double Spread { get; set; }
    }
}
