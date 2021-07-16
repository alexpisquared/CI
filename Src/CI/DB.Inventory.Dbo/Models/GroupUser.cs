using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class GroupUser
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public virtual User Group { get; set; }
        public virtual User User { get; set; }
    }
}
