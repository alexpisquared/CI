using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Securable
    {
        public int ObjectId { get; set; }
        public int KeyId { get; set; }
        public int GroupId { get; set; }
    }
}
