using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class SubType
    {
        public SubType()
        {
            Requests = new HashSet<Request>();
        }

        public int TypeId { get; set; }
        public int SubTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
