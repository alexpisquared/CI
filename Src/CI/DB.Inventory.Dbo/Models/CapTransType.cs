using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CapTransType
    {
        public CapTransType()
        {
            CapTrans = new HashSet<CapTran>();
        }

        public string TransType { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CapTran> CapTrans { get; set; }
    }
}
