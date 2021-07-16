using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountGroupSymbol
    {
        public int Acctid { get; set; }
        public string Usymbol { get; set; }

        public virtual Account Acct { get; set; }
    }
}
