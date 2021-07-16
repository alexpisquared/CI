using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class SecurityType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AbbrName { get; set; }
    }
}
