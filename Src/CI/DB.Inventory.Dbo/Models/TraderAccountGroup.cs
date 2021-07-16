using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TraderAccountGroup
    {
        public TraderAccountGroup()
        {
            TraderAccessGroups = new HashSet<TraderAccessGroup>();
            TraderAccountGroupMembers = new HashSet<TraderAccountGroupMember>();
        }

        public int TraderGroupId { get; set; }
        public string TraderGroupName { get; set; }
        public string Type { get; set; }

        public virtual ICollection<TraderAccessGroup> TraderAccessGroups { get; set; }
        public virtual ICollection<TraderAccountGroupMember> TraderAccountGroupMembers { get; set; }
    }
}
