using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class Source
    {
        public Source()
        {
            Requests = new HashSet<Request>();
        }

        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
