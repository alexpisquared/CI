using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }

        public virtual Application App { get; set; }
    }
}
