using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class RegisteredAccountFee
    {
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public double YearlyFeeAmt { get; set; }
        public string Prorated { get; set; }
        public string LastDayThreshold { get; set; }
        public double? LastDayThresholdAmt { get; set; }
        public double? MonthlyThresholdAmt { get; set; }
    }
}
