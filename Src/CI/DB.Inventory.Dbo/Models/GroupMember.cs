using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class GroupMember
    {
        public int BookId { get; set; }
        public int GroupId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookGroup Group { get; set; }
    }
}
