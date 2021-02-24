using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class RequestType
    {
        public RequestType()
        {
            Requests = new HashSet<Request>();
        }

        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
