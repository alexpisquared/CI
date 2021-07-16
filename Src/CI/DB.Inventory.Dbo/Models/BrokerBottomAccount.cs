using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BrokerBottomAccount
    {
        public string BrokerName { get; set; }
        public string SecurityType { get; set; }
        public string ExchangeCode { get; set; }
        public string AccountCode { get; set; }
        public string AdpaccountCode { get; set; }
    }
}
