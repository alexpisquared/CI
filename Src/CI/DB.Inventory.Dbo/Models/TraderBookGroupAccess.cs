using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderBookGroupAccess
    {
        public int BookGroupId { get; set; }
        public string TraderId { get; set; }

        public virtual BookGroup BookGroup { get; set; }
        public virtual Trader Trader { get; set; }
    }
}
