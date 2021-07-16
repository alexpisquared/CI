using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CcsecurityPosition
    {
        public int FileDateInt { get; set; }
        public string AcctNum { get; set; }
        public string SecNum { get; set; }
        public string CurrencyInd { get; set; }
        public int? Date { get; set; }
        public decimal? CurrentQty { get; set; }
        public decimal? SafeQty { get; set; }
        public decimal? PndQty { get; set; }
        public decimal? MktPrice { get; set; }
        public decimal? LoanRate { get; set; }
        public decimal? TdmktVal { get; set; }
        public decimal? TdloanVal { get; set; }
        public decimal? BookVal { get; set; }
        public decimal? SdmktVal { get; set; }
        public decimal? SdloanVal { get; set; }
        public string Cusip { get; set; }
        public string FundAcctNumber { get; set; }
        public long Id { get; set; }
    }
}
