using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CcsecMaster
    {
        public int FileDateInt { get; set; }
        public string SecNum { get; set; }
        public int? SecClass { get; set; }
        public string SecDescr1 { get; set; }
        public string SecDescr2 { get; set; }
        public string FundInd { get; set; }
        public decimal? Fraction { get; set; }
        public string CusipNum { get; set; }
        public string Symbol { get; set; }
        public string Rrspelignd { get; set; }
        public string CorpInd { get; set; }
        public string Dppelig { get; set; }
        public string Status { get; set; }
        public decimal? LastTrdPrice { get; set; }
        public string Market { get; set; }
        public int? PriceDate { get; set; }
        public decimal? AlterMktPrice { get; set; }
        public string AlterMktCode { get; set; }
        public int? AlterMktDate { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string SubSector { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? InterestRate2 { get; set; }
        public decimal? InterestRate3 { get; set; }
        public int? ExpiryDate { get; set; }
        public string Filler1 { get; set; }
        public string Filler2 { get; set; }
        public int? AccrualDate { get; set; }
        public string PaymentFreq { get; set; }
        public string TradesOption { get; set; }
        public string Coupons { get; set; }
        public int? ExpiryDate2 { get; set; }
        public decimal? StrikePrice { get; set; }
        public string UnderlyingCusip { get; set; }
        public string Oscexempt { get; set; }
        public string OptionType { get; set; }
        public string DiscountNote { get; set; }
        public string FixedRate { get; set; }
        public string FirstCouponDate { get; set; }
        public string Bbselig { get; set; }
        public string Cdselig { get; set; }
        public string Cnselig { get; set; }
        public string Vseelig { get; set; }
        public string Mdwelig { get; set; }
        public string EuroElig { get; set; }
        public string Qsspelig { get; set; }
        public string Nidselig { get; set; }
        public string DcsEcselig { get; set; }
        public string Csselig { get; set; }
        public string Qscexempt { get; set; }
        public string CertNumber { get; set; }
        public string CurrentPayableDate { get; set; }
        public decimal? CurrentDivRate { get; set; }
        public string DividendCurrency { get; set; }
        public string CurrentRecordDate { get; set; }
        public string ExDividendDate { get; set; }
        public decimal? DividendYield { get; set; }
        public string Symbol2 { get; set; }
        public string Code1 { get; set; }
        public long? Id { get; set; }
        public long CcsecId { get; set; }
    }
}
