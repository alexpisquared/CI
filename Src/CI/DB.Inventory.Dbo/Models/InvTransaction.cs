﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InvTransaction
    {
        public int TransactionId { get; set; }
        public string TransType { get; set; }
        public string Usymbol { get; set; }
        public int TradeDateInt { get; set; }
        public DateTime TradeDate { get; set; }
        public string Side { get; set; }
        public double Quantity { get; set; }
        public double AveragePrice { get; set; }
        public int BookId { get; set; }
        public double Commission { get; set; }
        public int SettlmntDateInt { get; set; }
        public string Currency { get; set; }
        public double? Spot { get; set; }
        public string Market { get; set; }
        public string CxlOrCorrect { get; set; }
        public DateTime? CxlOrCorrectDate { get; set; }
        public int? RefTransactionId { get; set; }
        public int BrokerAccountId { get; set; }
        public int? Reorg { get; set; }
        public int? ReorgDateInt { get; set; }
        public int? Journal { get; set; }
        public DateTime UpdateTime { get; set; }
        public int? UserIntId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BrokerAccount BrokerAccount { get; set; }
        public virtual InvTransOptionalDatum InvTransOptionalDatum { get; set; }
    }
}
