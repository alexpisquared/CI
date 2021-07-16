using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CashTran
    {
        public int CashTransId { get; set; }
        public int BookId { get; set; }
        public int TransType { get; set; }
        public double Amount { get; set; }
        public int TransDateInt { get; set; }
        public int SettlDateInt { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CxlOrCorrect { get; set; }
        public DateTime? CxlOrCorrectDate { get; set; }
        public string Comment { get; set; }
        public int? RefCashTransId { get; set; }

        public virtual Book Book { get; set; }
        public virtual CashTransType TransTypeNavigation { get; set; }
    }
}
