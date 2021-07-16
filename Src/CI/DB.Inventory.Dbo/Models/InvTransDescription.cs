using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InvTransDescription
    {
        public int InvTransDescriptionId { get; set; }
        public string Description { get; set; }
    }
}
