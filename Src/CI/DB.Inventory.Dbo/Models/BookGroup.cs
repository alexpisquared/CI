using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BookGroup
    {
        public BookGroup()
        {
            GroupMembers = new HashSet<GroupMember>();
            TraderBookGroupAccesses = new HashSet<TraderBookGroupAccess>();
        }

        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<TraderBookGroupAccess> TraderBookGroupAccesses { get; set; }
    }
}
