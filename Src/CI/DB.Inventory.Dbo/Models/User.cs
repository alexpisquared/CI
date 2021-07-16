using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class User
    {
        public User()
        {
            ApplicationGroupContexts = new HashSet<ApplicationGroupContext>();
            GroupUserGroups = new HashSet<GroupUser>();
            GroupUserUsers = new HashSet<GroupUser>();
            NewIssueAllocations = new HashSet<NewIssueAllocation>();
        }

        public string UserId { get; set; }
        public int AdminAccess { get; set; }
        public int UserIntId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public virtual ICollection<ApplicationGroupContext> ApplicationGroupContexts { get; set; }
        public virtual ICollection<GroupUser> GroupUserGroups { get; set; }
        public virtual ICollection<GroupUser> GroupUserUsers { get; set; }
        public virtual ICollection<NewIssueAllocation> NewIssueAllocations { get; set; }
    }
}
