using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BrokerDefaultHouse
    {
        public int BrokerAccountId { get; set; }
        public int HouseId { get; set; }

        public virtual BrokerAccount BrokerAccount { get; set; }
        public virtual BrokerAccount House { get; set; }
    }
}
