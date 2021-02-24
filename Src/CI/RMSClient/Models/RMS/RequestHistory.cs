using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class RequestHistory
    {
        public int RequestHistoryId { get; set; }
        public int? RequestId { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public int? LastShares { get; set; }
        public double? Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Bbsnote { get; set; }

        public virtual Request Request { get; set; }
    }
}
