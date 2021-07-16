using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ClientIdentifierAccountOverrideOld
    {
        public int Acctid { get; set; }
        public string OverrideOrderOrigination { get; set; }
        public string OverrideRoutingArrangementIndicator { get; set; }
        public string OverrideAlgorithmId { get; set; }
        public int? ApplyBrokerLeiId { get; set; }
        public int? ApplyCustomerLeiId { get; set; }
        public string Currency { get; set; }
    }
}
