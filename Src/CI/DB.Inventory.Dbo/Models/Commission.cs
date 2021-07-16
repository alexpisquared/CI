using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Commission
    {
        public int CommissionId { get; set; }
        public int AccountId { get; set; }
        public string TradeSource { get; set; }
        public string Exchange { get; set; }
        public string SecurityType { get; set; }
        public int TradeSessionId { get; set; }
        public double PriceRangeMin { get; set; }
        public double PriceRangeMax { get; set; }
        public double MinTicket { get; set; }
        public double MinPerTrade { get; set; }
        public double MaxPerTrade { get; set; }
        public double TicketCharge { get; set; }
        public bool AddRoutingFee { get; set; }
        public string AddRegulatoryExchangeFeeBeforeOrAfterMinMax { get; set; }
        public double PassiveFeePerShare { get; set; }
        public double AgressiveFeePerShare { get; set; }
        public double SellingRegulatoryPctFee { get; set; }
        public double ExecutionFeePerShare { get; set; }
        public double ExecutionFeePerTrade { get; set; }
        public double ExecutionFeePctPerTrade { get; set; }
        public double ExecutionFeePctPerValue { get; set; }
        public double MaxExecutionFeePctPerShare { get; set; }
        public double AnonymousFeePerShare { get; set; }
        public double IcebergFeePerShare { get; set; }
        public double ClearingFeePerShare { get; set; }
    }
}
