using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountGroupMember
    {
        public int GroupId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual AccountGroup Group { get; set; }
    }
}
