using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class UserAccessAccount
    {
        public int UserId { get; set; }
        public string LoginAcct { get; set; }
        public string PermissionRights { get; set; }
    }
}
