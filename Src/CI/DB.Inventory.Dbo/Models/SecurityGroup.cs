using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class SecurityGroup
    {
        public int SecGroupId { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
    }
}
