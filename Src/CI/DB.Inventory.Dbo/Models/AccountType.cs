using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? AdpType { get; set; }
        public string PlanType { get; set; }
        public string AcctTypeId { get; set; }
    }
}
