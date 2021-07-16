using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountSettlementCurrency
    {
        public int AccountId { get; set; }
        public string Currency { get; set; }

        public virtual Account Account { get; set; }
    }
}
