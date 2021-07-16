using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CommissionGroup
    {
        public CommissionGroup()
        {
            CommissionGroupMembers = new HashSet<CommissionGroupMember>();
            PromotionalCommissionGroupMembers = new HashSet<PromotionalCommissionGroupMember>();
        }

        public int CommissionGroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<CommissionGroupMember> CommissionGroupMembers { get; set; }
        public virtual ICollection<PromotionalCommissionGroupMember> PromotionalCommissionGroupMembers { get; set; }
    }
}
