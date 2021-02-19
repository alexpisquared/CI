using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class VwLast100
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Nickname { get; set; }
        public DateTime? DoneAtLocalTime { get; set; }
        public string EventData { get; set; }
    }
}
