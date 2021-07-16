using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ExchangeRoutingFee
    {
        public string Exchange { get; set; }
        public string DestBroker { get; set; }
        public string RoutingCode { get; set; }
        public double Fee { get; set; }
    }
}
