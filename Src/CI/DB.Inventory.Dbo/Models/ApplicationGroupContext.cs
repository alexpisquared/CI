using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ApplicationGroupContext
    {
        public int AppId { get; set; }
        public int GroupId { get; set; }

        public virtual Application App { get; set; }
        public virtual User Group { get; set; }
    }
}
