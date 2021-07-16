using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionColumnOverride
    {
        public int FileDefId { get; set; }
        public int FileColumnId { get; set; }

        public virtual TransactionFileColumn FileColumn { get; set; }
        public virtual TransactionFileDefinition FileDef { get; set; }
    }
}
