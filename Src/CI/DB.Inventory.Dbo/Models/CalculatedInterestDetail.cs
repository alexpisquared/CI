using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CalculatedInterestDetail
    {
        public int Id { get; set; }
        public int RootId { get; set; }
        public double SpreadUsed { get; set; }
        public string UnderlyingInterestCode { get; set; }
        public double UnderlyingInterestRate { get; set; }
        public double SettledCash { get; set; }

        public virtual CalculatedInterest Root { get; set; }
    }
}
