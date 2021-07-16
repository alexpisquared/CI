using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ResetCommissionGroupLog
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CommissionGroupId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
