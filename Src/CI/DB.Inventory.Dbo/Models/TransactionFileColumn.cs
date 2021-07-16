using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileColumn
    {
        public TransactionFileColumn()
        {
            TransactionColumnOverrides = new HashSet<TransactionColumnOverride>();
            TransactionFileColumnDefaults = new HashSet<TransactionFileColumnDefault>();
        }

        public int FileColumnId { get; set; }
        public string FileTypeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ColumnOrder { get; set; }
        public string IncludeInUpload { get; set; }
        public string UseInTotals { get; set; }

        public virtual TransactionFileType FileType { get; set; }
        public virtual ICollection<TransactionColumnOverride> TransactionColumnOverrides { get; set; }
        public virtual ICollection<TransactionFileColumnDefault> TransactionFileColumnDefaults { get; set; }
    }
}
