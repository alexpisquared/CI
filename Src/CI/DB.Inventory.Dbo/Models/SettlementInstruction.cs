using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class SettlementInstruction
    {
        public int InstructionId { get; set; }
        public string Usymbol { get; set; }
        public DateTime TradeDateStart { get; set; }
        public DateTime TradeDateEnd { get; set; }
        public string SettlmntTyp { get; set; }
        public DateTime? FutSettDate { get; set; }
    }
}
