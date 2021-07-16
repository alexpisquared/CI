using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class PermissionAssignment
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public string Status { get; set; }
        public long TblId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }
}
