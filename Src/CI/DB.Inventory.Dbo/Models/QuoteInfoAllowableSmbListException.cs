using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class QuoteInfoAllowableSmbListException
    {
        public string Symbolstring { get; set; }
        public string Exchange { get; set; }
        public string AddOrDelete { get; set; }
    }
}
