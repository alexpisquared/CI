using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CalculatedInterest
    {
        public CalculatedInterest()
        {
            CalculatedInterestDetails = new HashSet<CalculatedInterestDetail>();
        }

        public int DateInt { get; set; }
        public string AccountId { get; set; }
        public string Currency { get; set; }
        public string InterestCode { get; set; }
        public double SettledCash { get; set; }
        public double? CalculatedInterest1 { get; set; }
        public int Id { get; set; }

        public virtual ICollection<CalculatedInterestDetail> CalculatedInterestDetails { get; set; }
    }
}
