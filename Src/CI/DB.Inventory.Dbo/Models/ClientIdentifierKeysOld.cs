using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ClientIdentifierKeysOld
    {
        public int LeiId { get; set; }
        public string Name { get; set; }
        public byte? Encrypt { get; set; }
        public string BrokerLeikey { get; set; }
        public string CustomerLeikey { get; set; }
        public string UniqueDealerId { get; set; }
        public string KeyStartDate { get; set; }
        public string KeyEndDate { get; set; }
        public string LeiType { get; set; }
    }
}
