using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Cctrade
    {
        public int FileDateInt { get; set; }
        public string AcctNum { get; set; }
        public int? TradeNum { get; set; }
        public string BuySellInd { get; set; }
        public string CancelInd { get; set; }
        public string Rrcode { get; set; }
        public string NumMf { get; set; }
        public string SecNum { get; set; }
        public int? TradeDate { get; set; }
        public int? ValDate { get; set; }
        public string Mkt { get; set; }
        public int? ProcDate { get; set; }
        public string CusipNum { get; set; }
        public decimal? Forex { get; set; }
        public decimal? Price { get; set; }
        public decimal? Qty { get; set; }
        public decimal? GrossAmount { get; set; }
        public string CommPrefigInd { get; set; }
        public decimal? CommAmount { get; set; }
        public decimal? CommissionRr { get; set; }
        public decimal? Sfamount { get; set; }
        public decimal? ExcAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? IntAmount { get; set; }
        public int? OrderNum { get; set; }
        public string BrnCd { get; set; }
        public int? CxlTradeNum { get; set; }
        public int? CxlDate { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Dsccharge { get; set; }
        public string Currency { get; set; }
        public string OrderSource { get; set; }
        public long Id { get; set; }
    }
}
