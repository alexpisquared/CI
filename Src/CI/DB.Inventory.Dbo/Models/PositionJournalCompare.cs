using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class PositionJournalCompare
    {
        public string Usymbol { get; set; }
        public double? QtyWithoutJournal { get; set; }
        public double? Quantity { get; set; }
    }
}
