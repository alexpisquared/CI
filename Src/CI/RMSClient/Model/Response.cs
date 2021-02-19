using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Model
{
    public partial class Response
    {
        public int RespId { get; set; }
        public string RespText { get; set; }
        public string Cause { get; set; }
    }
}
