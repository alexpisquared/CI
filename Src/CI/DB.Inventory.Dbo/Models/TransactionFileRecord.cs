using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileRecord
    {
        public int FileRecordId { get; set; }
        public int TransactionFileId { get; set; }
        public int RecordTypeId { get; set; }
        public string RecordData { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Excluded { get; set; }
        public string Note { get; set; }

        public virtual TransactionFileRecordType RecordType { get; set; }
        public virtual TransactionFile TransactionFile { get; set; }
    }
}
