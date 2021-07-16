using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderAccessGroup
    {
        public int TraderGroupId { get; set; }
        public int AccountId { get; set; }
        public bool IsDefault { get; set; }
        public string Currency { get; set; }

        public virtual Account Account { get; set; }
        public virtual TraderAccountGroup TraderGroup { get; set; }
    }
}
