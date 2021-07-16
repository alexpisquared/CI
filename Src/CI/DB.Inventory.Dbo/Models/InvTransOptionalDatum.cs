using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class InvTransOptionalDatum
    {
        public int TransactionId { get; set; }
        public string TransDesc { get; set; }
        public string RelatedUsymbol { get; set; }
        public double? AccruedInterest { get; set; }
        public bool? Uploaded { get; set; }
        public bool? ManualInterest { get; set; }
        public int? AllocationId { get; set; }
        public int? UploadDateInt { get; set; }

        public virtual InvTransaction Transaction { get; set; }
    }
}
