using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class EodreportHistory
    {
        public int FileId { get; set; }
        public int FileDefId { get; set; }
        public DateTime Date { get; set; }

        public virtual Eodreport FileDef { get; set; }
    }
}
