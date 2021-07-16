using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CcoptionsActivity
    {
        public int FileDateInt { get; set; }
        public DateTime? ProcDt { get; set; }
        public DateTime? TrdDt { get; set; }
        public DateTime? StlDt { get; set; }
        public string Acct { get; set; }
        public string Cusip { get; set; }
        public string ShortName { get; set; }
        public DateTime? ExpDt { get; set; }
        public decimal? StrikePrc { get; set; }
        public int? Qty { get; set; }
        public decimal? Amt { get; set; }
        public string TranCode { get; set; }
        public string FundType { get; set; }
        public string MktCode { get; set; }
        public long Id { get; set; }
    }
}
