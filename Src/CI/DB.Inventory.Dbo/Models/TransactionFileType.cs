using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileType
    {
        public TransactionFileType()
        {
            TransactionFileColumns = new HashSet<TransactionFileColumn>();
            TransactionFileDefinitions = new HashSet<TransactionFileDefinition>();
        }

        public string FileTypeId { get; set; }
        public string FileTypeDescription { get; set; }

        public virtual ICollection<TransactionFileColumn> TransactionFileColumns { get; set; }
        public virtual ICollection<TransactionFileDefinition> TransactionFileDefinitions { get; set; }
    }
}
