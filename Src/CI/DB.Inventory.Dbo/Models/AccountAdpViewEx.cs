using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountAdpViewEx
    {
        public int AccountId { get; set; }
        public string ShortName { get; set; }
        public string PensonAccountId { get; set; }
        public string PensonUsaccountId { get; set; }
        public string PensonGiveUpId { get; set; }
        public string BmoaccountId { get; set; }
        public string TsxStampId { get; set; }
        public string DefaultBook { get; set; }
        public string AccountTypes { get; set; }
        public string DefaultAccountType { get; set; }
        public string OverrideAccountType { get; set; }
        public int? AcctBrokerId { get; set; }
        public int? IntRateId { get; set; }
        public int? BorRateId { get; set; }
        public int? FxRateId { get; set; }
        public string AcctTypeId { get; set; }
        public int? SecGroupId { get; set; }
        public string AdpAccountCd { get; set; }
        public string AdpBranchCd { get; set; }
        public string AdpTypeAccountCd { get; set; }
        public string AdpChk { get; set; }
        public string CanaccordAccountId { get; set; }
        public string AdpaccountCode { get; set; }
        public string Currency { get; set; }
    }
}
