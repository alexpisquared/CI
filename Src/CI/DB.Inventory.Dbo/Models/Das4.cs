using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Das4
    {
        public string AccountCurrency { get; set; }
        public string Symbol { get; set; }
        public string AccountNumber { get; set; }
        public string Ismcode { get; set; }
        public string SecurityDesc { get; set; }
        public string SecurityCurrency { get; set; }
        public string SecurityGroupCode { get; set; }
        public int LastTradeDate { get; set; }
        public int CurrentQty { get; set; }
        public int SfkQty { get; set; }
        public int PendingQty { get; set; }
        public double MarketPx { get; set; }
        public double LoanValue { get; set; }
        public double AvgUnitPx { get; set; }
        public int ProcessDate { get; set; }
        public DateTime InsertTime { get; set; }
        public int Id { get; set; }
    }
}
