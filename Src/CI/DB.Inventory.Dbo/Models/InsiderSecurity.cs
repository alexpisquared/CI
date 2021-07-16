using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InsiderSecurity
    {
        public int AccountId { get; set; }
        public string Usymbol { get; set; }
        public string RegulationId { get; set; }

        public virtual Account Account { get; set; }
        public virtual CompanyProfile UsymbolNavigation { get; set; }
    }
}
