using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ClientIdentifierAccountOverride
    {
        public int Acctid { get; set; }
        public string OverrideOrderOrigination { get; set; }
        public string OverrideRoutingArrangementIndicator { get; set; }
        public string OverrideAlgorithmId { get; set; }
        public string OverrideCustomerAccount { get; set; }
        public string OverrideCustomerLei { get; set; }
        public string OverrideBrokerLei { get; set; }
        public string DefaultOrderOrigination { get; set; }
        public string DefaultRoutingArrangementIndicator { get; set; }
        public string DefaultAlgorithmId { get; set; }
        public string DefaultCustomerAccount { get; set; }
        public string DefaultCustomerLei { get; set; }
        public string DefaultBrokerLei { get; set; }
    }
}
