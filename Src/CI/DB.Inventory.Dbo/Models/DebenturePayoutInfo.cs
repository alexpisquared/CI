using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class DebenturePayoutInfo
    {
        public int CouponDateId { get; set; }
        public string Usymbol { get; set; }
        public int CouponDate { get; set; }
        public int RecordDate { get; set; }

        public virtual DebentureProfile UsymbolNavigation { get; set; }
    }
}
