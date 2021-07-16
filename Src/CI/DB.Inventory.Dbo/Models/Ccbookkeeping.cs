using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Ccbookkeeping
    {
        public int FileDateInt { get; set; }
        public string AcctNum { get; set; }
        public string TradeDate { get; set; }
        public string SecNum { get; set; }
        public decimal? AccruedInt { get; set; }
        public string Rrcode { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public string TradeNumber { get; set; }
        public string TransCode { get; set; }
        public string Interface { get; set; }
        public string ProcDate { get; set; }
        public string Currency { get; set; }
        public string SettleDate { get; set; }
        public string Description { get; set; }
        public decimal? FillPrice { get; set; }
        public decimal? GrossTradeAmt { get; set; }
        public string Cusip { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Rrcommission { get; set; }
        public decimal? CustCommission { get; set; }
        public string Pacid { get; set; }
        public string JournalRefNum { get; set; }
        public long Id { get; set; }
    }
}
