using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CcclientBalance
    {
        public string AcctNum { get; set; }
        public decimal? TtlMonthEndMv { get; set; }
        public decimal? TdcashBalance { get; set; }
        public decimal? SdcashBalance { get; set; }
        public decimal? TdshortPosMv { get; set; }
        public decimal? TdmarketValue { get; set; }
        public decimal? SfkPosMv { get; set; }
        public decimal? TtlLoanValue { get; set; }
        public decimal? Tdequity { get; set; }
        public decimal? TtlBookValue { get; set; }
        public decimal? SdshortPosMv { get; set; }
        public decimal? SdmarketValue { get; set; }
        public decimal? Sdequity { get; set; }
        public int FileDateInt { get; set; }
        public long Id { get; set; }
    }
}
