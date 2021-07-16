using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Das1
    {
        public int DateInt { get; set; }
        public string AccountNumber { get; set; }
        public string GuranteeAccount { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public string Rr { get; set; }
        public string AccountClass { get; set; }
        public string OptionCode { get; set; }
        public string DivConfirm { get; set; }
        public string DebitCode { get; set; }
        public string Interestcode { get; set; }
        public string ResidencyCode { get; set; }
        public string NonRes { get; set; }
        public string Name { get; set; }
        public double CurrentTdbalance { get; set; }
        public double CurrentSdbalance { get; set; }
        public double ValueOfShorts { get; set; }
        public double MarketValueOfPos { get; set; }
        public double LoanValue { get; set; }
        public double RequiredMargin { get; set; }
        public double RequiredSecure { get; set; }
        public double Margin { get; set; }
        public int ProcessDate { get; set; }
        public string ClientId { get; set; }
        public double Equity { get; set; }
        public double AlternateEquity { get; set; }
        public DateTime InsertTime { get; set; }
        public int Id { get; set; }
    }
}
