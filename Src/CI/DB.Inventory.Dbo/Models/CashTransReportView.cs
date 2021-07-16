using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CashTransReportView
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string TransTypeString { get; set; }
        public DateTime? TransDate { get; set; }
        public int TransDateInt { get; set; }
        public DateTime? SettlDate { get; set; }
        public int SettlDateInt { get; set; }
        public double Amount { get; set; }
        public string Comment { get; set; }
        public int TransType { get; set; }
    }
}
