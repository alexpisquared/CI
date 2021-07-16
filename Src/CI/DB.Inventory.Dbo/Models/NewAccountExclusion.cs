using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class NewAccountExclusion
    {
        public string BranchCd { get; set; }
        public string AccountCd { get; set; }
        public string TypeAccountCd { get; set; }
    }
}
