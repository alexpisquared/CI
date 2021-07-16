using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class UserSecGroupLevel
    {
        public int Userid { get; set; }
        public int Secgroupid { get; set; }

        public virtual UserAccessAccount User { get; set; }
    }
}
