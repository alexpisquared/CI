using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Trader
    {
        public Trader()
        {
            TraderBookGroupAccesses = new HashSet<TraderBookGroupAccess>();
        }

        public string TraderId { get; set; }
        public string TraderType { get; set; }
        public string TraderDescription { get; set; }

        public virtual ICollection<TraderBookGroupAccess> TraderBookGroupAccesses { get; set; }
    }
}
