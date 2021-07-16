using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class NewIssueAllocation
    {
        public int AllocationId { get; set; }
        public double Fill { get; set; }
        public int BookId { get; set; }
        public string CxlOrCorrect { get; set; }
        public DateTime? CxlOrCorrectDate { get; set; }
        public int? RefTransactionId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int? UserIntId { get; set; }
        public int NewIssueId { get; set; }
        public int? UploadedOn { get; set; }
        public int? ImportedOn { get; set; }
        public int UploadDateInt { get; set; }
        public int? SettlementDateInt { get; set; }

        public virtual Book Book { get; set; }
        public virtual NewIssue NewIssue { get; set; }
        public virtual User UserInt { get; set; }
    }
}
