using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountCurrencyDetails = new HashSet<AccountCurrencyDetail>();
            AccountGroupMembers = new HashSet<AccountGroupMember>();
            AccountGroupSymbols = new HashSet<AccountGroupSymbol>();
            AccountSettlementCurrencies = new HashSet<AccountSettlementCurrency>();
            CommissionGroupMembers = new HashSet<CommissionGroupMember>();
            CurrencySpreadGroupMembers = new HashSet<CurrencySpreadGroupMember>();
            InsiderSecurities = new HashSet<InsiderSecurity>();
            NewAccountDefaults = new HashSet<NewAccountDefault>();
            PromotionalCommissionGroupMembers = new HashSet<PromotionalCommissionGroupMember>();
            TraderAccessGroups = new HashSet<TraderAccessGroup>();
            TraderAccounts = new HashSet<TraderAccount>();
        }

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

        public virtual ICollection<AccountCurrencyDetail> AccountCurrencyDetails { get; set; }
        public virtual ICollection<AccountGroupMember> AccountGroupMembers { get; set; }
        public virtual ICollection<AccountGroupSymbol> AccountGroupSymbols { get; set; }
        public virtual ICollection<AccountSettlementCurrency> AccountSettlementCurrencies { get; set; }
        public virtual ICollection<CommissionGroupMember> CommissionGroupMembers { get; set; }
        public virtual ICollection<CurrencySpreadGroupMember> CurrencySpreadGroupMembers { get; set; }
        public virtual ICollection<InsiderSecurity> InsiderSecurities { get; set; }
        public virtual ICollection<NewAccountDefault> NewAccountDefaults { get; set; }
        public virtual ICollection<PromotionalCommissionGroupMember> PromotionalCommissionGroupMembers { get; set; }
        public virtual ICollection<TraderAccessGroup> TraderAccessGroups { get; set; }
        public virtual ICollection<TraderAccount> TraderAccounts { get; set; }
    }
}
