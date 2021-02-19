using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class VwUser
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastEvEst { get; set; }
        public int? Cnt { get; set; }
        public string Nickname { get; set; }
        public string EventData { get; set; }
    }
}
