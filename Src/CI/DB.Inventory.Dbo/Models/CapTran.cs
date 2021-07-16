using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CapTran
    {
        public int CapTransId { get; set; }
        public int BookId { get; set; }
        public string Usymbol { get; set; }
        public string TransType { get; set; }
        public int? TransDateInt { get; set; }
        public int? ExDivDateInt { get; set; }
        public int? PaymentDateInt { get; set; }
        public int? RecordDateInt { get; set; }
        public int? Redemption { get; set; }
        public double? Adjustment { get; set; }
        public double? DivPerShare { get; set; }
        public double? OrigDivPerShare { get; set; }
        public int? NumberOfShares { get; set; }
        public double? AveragePrice { get; set; }
        public string Currency { get; set; }
        public double? Fxrate { get; set; }
        public string CxlorCorrect { get; set; }
        public DateTime? CxlOrCorrectDate { get; set; }
        public int? RefCapTransId { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Book Book { get; set; }
        public virtual CapTransType TransTypeNavigation { get; set; }
        public virtual CompanyProfile UsymbolNavigation { get; set; }
    }
}
