using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CapTransHist
    {
        public int CapTransHistId { get; set; }
        public int BookId { get; set; }
        public string TransType { get; set; }
        public decimal? Mtd { get; set; }
        public decimal? Ytd { get; set; }
        public decimal? Ltd { get; set; }
        public decimal? Today { get; set; }
        public decimal? Yesterday { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Book Book { get; set; }
    }
}
