using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CurrencySpreadGroupMember
    {
        public int CurrencySpreadGroupId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual CurrencySpreadGroup CurrencySpreadGroup { get; set; }
    }
}
