using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AuditTrailObject
    {
        public int AuditObjectId { get; set; }
        public string Name { get; set; }
    }
}
