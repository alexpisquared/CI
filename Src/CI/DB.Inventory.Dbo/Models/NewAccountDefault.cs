using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class NewAccountDefault
    {
        public string AccountType { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
