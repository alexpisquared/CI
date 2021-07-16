using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Bk
    {
        public int DateInt { get; set; }
        public string AccountNumber { get; set; }
        public int ProcessDateInt { get; set; }
        public int TradeDateInt { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDesc { get; set; }
        public double AmountValue { get; set; }
        public double MiscAmountValue { get; set; }
        public string Currency { get; set; }
        public double Px { get; set; }
        public double Volume { get; set; }
        public string MarketCode { get; set; }
        public string TransactionId { get; set; }
        public string TransactionSubId { get; set; }
        public DateTime InsertTime { get; set; }
        public int Id { get; set; }
    }
}
