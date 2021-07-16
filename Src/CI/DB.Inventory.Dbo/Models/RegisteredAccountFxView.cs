using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class RegisteredAccountFxView
    {
        public int Tradedateint { get; set; }
        public string Currency { get; set; }
        public string Pensonaccountid { get; set; }
        public double? Fxrate { get; set; }
    }
}
