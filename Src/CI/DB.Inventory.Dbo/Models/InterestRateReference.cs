using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InterestRateReference
    {
        public string InterestRateCode { get; set; }
        public string Currency { get; set; }
        public int DateInt { get; set; }
        public double Interest { get; set; }
    }
}
