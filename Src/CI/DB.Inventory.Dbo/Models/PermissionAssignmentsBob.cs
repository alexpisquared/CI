using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class PermissionAssignmentsBob
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public string Status { get; set; }
    }
}
