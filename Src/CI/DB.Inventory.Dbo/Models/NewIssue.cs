using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class NewIssue
    {
        public NewIssue()
        {
            NewIssueAllocations = new HashSet<NewIssueAllocation>();
        }

        public int NewIssueId { get; set; }
        public string Usymbol { get; set; }
        public int CounterPartyId { get; set; }
        public int HouseId { get; set; }
        public int SettlmntDateInt { get; set; }
        public double IssuePx { get; set; }
        public double DrawdownPx { get; set; }
        public double FillFromLead { get; set; }
        public bool? Active { get; set; }
        public int CreateDateInt { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool? UploadInitialAlloc { get; set; }
        public int SymbolReference { get; set; }

        public virtual Book CounterParty { get; set; }
        public virtual Book House { get; set; }
        public virtual ICollection<NewIssueAllocation> NewIssueAllocations { get; set; }
    }
}
