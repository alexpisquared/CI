using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class AuditTrail
    {
        public int AuditTrailId { get; set; }
        public int AuditObjectId { get; set; }
        public string AuditKeyId { get; set; }
        public DateTime DtStamp { get; set; }
        public string UserId { get; set; }
        public string Xmlchanges { get; set; }
        public string AuditObjectKey { get; set; }
    }
}
