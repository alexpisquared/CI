using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class LockInfo
    {
        public string LockId { get; set; }
        public bool? Locked { get; set; }
        public string UserId { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
