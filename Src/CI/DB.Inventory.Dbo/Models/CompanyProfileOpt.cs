using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CompanyProfileOpt
    {
        public string Usymbol { get; set; }
        public string OptionType { get; set; }
        public int MaturityMonthYear { get; set; }
        public int PutOrCall { get; set; }
        public double StrikePrice { get; set; }

        public virtual CompanyProfile UsymbolNavigation { get; set; }
    }
}
