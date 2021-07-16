using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CapTransReportView
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Symbol { get; set; }
        public string TransTypeString { get; set; }
        public string TransType { get; set; }
        public int? UpdateTimeInt { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentDateInt { get; set; }
        public int? RecordDateInt { get; set; }
        public DateTime? RecordDate { get; set; }
        public int? NumberOfShares { get; set; }
        public double? DivPerShare { get; set; }
        public double? Cash { get; set; }
        public string Currency { get; set; }
        public int? ExDivDateInt { get; set; }
        public DateTime? ExDivDate { get; set; }
        public string BookCurrency { get; set; }
        public int? TransDateInt { get; set; }
        public DateTime? TransDate { get; set; }
    }
}
