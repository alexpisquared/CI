using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ImportLog
    {
        public string ImportType { get; set; }
        public DateTime ImportTime { get; set; }
        public int StartId { get; set; }
        public int EndId { get; set; }
        public string UserId { get; set; }
    }
}
