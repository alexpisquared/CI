using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.BR
{
    public partial class Account
    {
        public long AccountId { get; set; }
        public long ClientId { get; set; }
        public DateTime? UpdtTmstp { get; set; }
        public string UpdtNm { get; set; }
        public DateTime? OpenedDt { get; set; }
        public DateTime? ClosedDt { get; set; }
        public string BranchCd { get; set; }
        public string AccountCd { get; set; }
        public string TypeAccountCd { get; set; }
        public string PlanTypeCd { get; set; }
        public string SpsEverInd { get; set; }
        public string StmtPrintFlg { get; set; }
        public string RepCd { get; set; }
        public string AtonRefId { get; set; }
        public string NicknameTxt { get; set; }
        public string SubTypeCd { get; set; }
        public string CmmsnToBrokerCd { get; set; }
        public decimal BuyingPowerMultiple { get; set; }
        public string LeverageDisallowedCd { get; set; }
        public string AccountNum { get; set; }
        public string HouseMarginType { get; set; }
        public string RespClbFlg { get; set; }
        public string RespAcesgFlg { get; set; }
        public string RespCesgFlg { get; set; }
        public string Jurisdiction { get; set; }
        public string CodCuid { get; set; }
        public string CodOthBrkrAccountNum { get; set; }
        public string ClientStatus { get; set; }
        public string ReferralSite { get; set; }
        public string CampaignName { get; set; }
        public string AdGroup { get; set; }
        public string AdvertisementSource { get; set; }
        public string PromotionCodeOnOpeningAccount { get; set; }
        public string AccountReferralPersonInternal { get; set; }
        public string AccountReferralPersonExternal { get; set; }
        public DateTime? TimeAccountOpeningStartedByClient { get; set; }
        public DateTime? TimeAccountWasApproved { get; set; }
        public string WhereClientHeardAboutUs { get; set; }
        public bool? EDocumentAccountOpening { get; set; }
        public string StatusCd { get; set; }
    }
}
