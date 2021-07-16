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
    }
}
