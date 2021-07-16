using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Sl
    {
        public string AccountNumber { get; set; }
        public string IbmSecurityId { get; set; }
        public int ProcessDate { get; set; }
        public string SecurityFunds { get; set; }
        public string SecurityType { get; set; }
        public string SecurityClass { get; set; }
        public string CusipNo { get; set; }
        public string Symbol { get; set; }
        public string EngShortDesc { get; set; }
        public int Quantity { get; set; }
        public double? MarketPrice { get; set; }
        public double CalcPrice { get; set; }
        public double Capital { get; set; }
        public int DayYear { get; set; }
        public double OvernightRate { get; set; }
        public double? CalcRateA { get; set; }
        public double CalcRates { get; set; }
        public double ChargeA { get; set; }
        public double Charges { get; set; }
        public int NoDays { get; set; }
        public double ChargeTotalTran { get; set; }
        public DateTime InsertTime { get; set; }
        public double? BorrowRate { get; set; }
        public double? BbscalcPrice { get; set; }
        public double? MinRate { get; set; }
        public double? MinSpread { get; set; }
        public double? PctSpread { get; set; }
        public string AccountCredited { get; set; }
        public double? Fee { get; set; }
        public int Id { get; set; }
        public string SecurityAdpNbr { get; set; }
        public double? Minloanet { get; set; }
        public string BrAcctType { get; set; }
    }
}
