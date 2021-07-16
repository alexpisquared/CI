using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFile
    {
        public TransactionFile()
        {
            TransactionFileRecords = new HashSet<TransactionFileRecord>();
        }

        public int FileId { get; set; }
        public int FileDefId { get; set; }
        public int CreatorId { get; set; }
        public int? SupervisorId { get; set; }
        public int? ManagerId { get; set; }
        public int? UploadId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UploadTime { get; set; }
        public DateTime? SupervisorApprovalTime { get; set; }
        public DateTime? ManagerApprovalTime { get; set; }
        public string Status { get; set; }

        public virtual TransactionFileDefinition FileDef { get; set; }
        public virtual ICollection<TransactionFileRecord> TransactionFileRecords { get; set; }
    }
}
