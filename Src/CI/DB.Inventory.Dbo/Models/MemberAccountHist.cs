using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class MemberAccountHist
    {
        public int MemberId { get; set; }
        public int AccountId { get; set; }
        public string ChangeTypeCd { get; set; }
        public DateTime ChangeDt { get; set; }
        public string ChangeUserNm { get; set; }
    }
}
