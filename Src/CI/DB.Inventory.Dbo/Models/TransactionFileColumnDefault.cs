using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileColumnDefault
    {
        public int FileColumnId { get; set; }
        public int FileDefId { get; set; }
        public string FieldDefault { get; set; }

        public virtual TransactionFileColumn FileColumn { get; set; }
        public virtual TransactionFileDefinition FileDef { get; set; }
    }
}
