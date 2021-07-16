using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class LockedRecord
    {
        public DateTime LockedEndDate { get; set; }
    }
}
