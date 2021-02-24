using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.Inventory
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string ReportingCurrency { get; set; }
        public string AccountNum { get; set; }
        public string DefMarket { get; set; }
        public string ShortName { get; set; }
        public int StrategyType { get; set; }
        public int Active { get; set; }
    }
}
