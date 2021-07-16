using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CommissionGroupMember
    {
        public int CommissionGroupId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual CommissionGroup CommissionGroup { get; set; }
    }
}
