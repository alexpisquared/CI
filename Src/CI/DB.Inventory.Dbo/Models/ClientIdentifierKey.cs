using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ClientIdentifierKey
    {
        public int LeiId { get; set; }
        public string Name { get; set; }
        public string LeiencrptionKey { get; set; }
        public string UniqueDealerId { get; set; }
        public string KeyStartDate { get; set; }
        public string KeyEndDate { get; set; }
        public int Enabled { get; set; }
    }
}
