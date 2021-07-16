using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AccountGroup
    {
        public AccountGroup()
        {
            AccountGroupMembers = new HashSet<AccountGroupMember>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int VbloginId { get; set; }
        public int? Startdate { get; set; }
        public int? Enddate { get; set; }
        public int? NumTrades { get; set; }
        public int? NumEquity { get; set; }
        public int? NumDebenture { get; set; }
        public int? NumNote { get; set; }
        public int? NumOpt { get; set; }
        public int? NumForEx { get; set; }
        public int? NumFuture { get; set; }
        public int? NumMleg { get; set; }
        public bool? BEquity { get; set; }
        public bool? BDebenture { get; set; }
        public bool? BNote { get; set; }
        public bool? BOpt { get; set; }
        public bool? BForEx { get; set; }
        public bool? BFuture { get; set; }
        public bool? BMleg { get; set; }

        public virtual ICollection<AccountGroupMember> AccountGroupMembers { get; set; }
    }
}
