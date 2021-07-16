using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Permission
    {
        public Permission()
        {
            PermissionAssignments = new HashSet<PermissionAssignment>();
        }

        public int PermissionId { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }

        public virtual Application App { get; set; }
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }
    }
}
