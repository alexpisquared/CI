using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountVbloginMap
    {
        public int AccountId { get; set; }
        public string VbLoginId { get; set; }
    }
}
