using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CurrencySpreadGroup
    {
        public CurrencySpreadGroup()
        {
            CurrencySpreadGroupMembers = new HashSet<CurrencySpreadGroupMember>();
            CurrencySpreads = new HashSet<CurrencySpread>();
        }

        public int CurrencySpreadGroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<CurrencySpreadGroupMember> CurrencySpreadGroupMembers { get; set; }
        public virtual ICollection<CurrencySpread> CurrencySpreads { get; set; }
    }
}
