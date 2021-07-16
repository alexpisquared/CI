using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class DebentureProfile
    {
        public DebentureProfile()
        {
            DebenturePayoutInfos = new HashSet<DebenturePayoutInfo>();
        }

        public string Usymbol { get; set; }
        public byte CouponFrequency { get; set; }
        public int MaturityDate { get; set; }
        public byte DayCounting { get; set; }
        public int InterestStartDate { get; set; }
        public double CouponRate { get; set; }
        public int FirstCouponDate { get; set; }

        public virtual CompanyProfile UsymbolNavigation { get; set; }
        public virtual ICollection<DebenturePayoutInfo> DebenturePayoutInfos { get; set; }
    }
}
