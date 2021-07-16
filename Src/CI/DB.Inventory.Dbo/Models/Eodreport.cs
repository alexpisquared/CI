using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Eodreport
    {
        public Eodreport()
        {
            EodreportHistories = new HashSet<EodreportHistory>();
        }

        public int FileDefId { get; set; }
        public int Order { get; set; }
        public string FileDescription { get; set; }

        public virtual ICollection<EodreportHistory> EodreportHistories { get; set; }
    }
}
