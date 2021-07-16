using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class RoutingInstruction
    {
        public int RoutingInstructionId { get; set; }
        public int AccountId { get; set; }
        public string Exchange { get; set; }
        public string DefaultInstruction { get; set; }
        public string OverrideInstruction { get; set; }
    }
}
