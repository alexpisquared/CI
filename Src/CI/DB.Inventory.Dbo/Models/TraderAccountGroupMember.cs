using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderAccountGroupMember
    {
        public int TraderGroupId { get; set; }
        public string TraderId { get; set; }

        public virtual TraderAccountGroup TraderGroup { get; set; }
    }
}
