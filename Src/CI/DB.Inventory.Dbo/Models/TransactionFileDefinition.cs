using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class TransactionFileDefinition
    {
        public TransactionFileDefinition()
        {
            TransactionColumnOverrides = new HashSet<TransactionColumnOverride>();
            TransactionFileColumnDefaults = new HashSet<TransactionFileColumnDefault>();
            TransactionFiles = new HashSet<TransactionFile>();
        }

        public int FileDefId { get; set; }
        public string FileTypeId { get; set; }
        public string FileDescription { get; set; }
        public int CreateGroupId { get; set; }
        public int? SupervisorGroupId { get; set; }
        public int? ManagerGroupId { get; set; }

        public virtual TransactionFileType FileType { get; set; }
        public virtual ICollection<TransactionColumnOverride> TransactionColumnOverrides { get; set; }
        public virtual ICollection<TransactionFileColumnDefault> TransactionFileColumnDefaults { get; set; }
        public virtual ICollection<TransactionFile> TransactionFiles { get; set; }
    }
}
