using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountTypeExternalMap
    {
        public string ExAcctTypeId { get; set; }
        public string Currency { get; set; }
        public string AcctTypeId { get; set; }
    }
}
