using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileRecordType
    {
        public TransactionFileRecordType()
        {
            TransactionFileRecords = new HashSet<TransactionFileRecord>();
        }

        public int RecordTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TransactionFileRecord> TransactionFileRecords { get; set; }
    }
}
