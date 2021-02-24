using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class Status
    {
        public Status()
        {
            Requests = new HashSet<Request>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
