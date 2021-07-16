using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationGroupContexts = new HashSet<ApplicationGroupContext>();
            Permissions = new HashSet<Permission>();
        }

        public int AppId { get; set; }
        public string AppName { get; set; }

        public virtual ICollection<ApplicationGroupContext> ApplicationGroupContexts { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
