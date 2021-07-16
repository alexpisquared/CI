using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountCurrencyDetail
    {
        public int Acctid { get; set; }
        public string Currency { get; set; }
        public string OverrideAccountType { get; set; }
        public string AccountTypes { get; set; }
        public string TsxStampId { get; set; }
        public string TraderId { get; set; }
        public int? ShortMarkingExempt { get; set; }

        public virtual Account Acct { get; set; }
    }
}
