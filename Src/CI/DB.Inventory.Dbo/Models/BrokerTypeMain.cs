using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BrokerTypeMain
    {
        public byte[] BrokerTypeId { get; set; }
        public string BrokerTypeDesc { get; set; }
    }
}
