using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class EventHistoryTracker
    {
        public int Id { get; set; }
        public DateTime DtStamp { get; set; }
        public string UserId { get; set; }
        public string AcctEventId { get; set; }
        public string Updatedfield { get; set; }
        public int? EventTypeId { get; set; }
    }
}
