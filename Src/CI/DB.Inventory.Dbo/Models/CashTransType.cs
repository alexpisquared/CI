using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CashTransType
    {
        public CashTransType()
        {
            CashTrans = new HashSet<CashTran>();
        }

        public int TransType { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CashTran> CashTrans { get; set; }
    }
}
