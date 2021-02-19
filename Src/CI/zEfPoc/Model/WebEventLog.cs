using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class WebEventLog
    {
        public int Id { get; set; }
        public int WebsiteUserId { get; set; }
        public string EventName { get; set; }
        public DateTime DoneAt { get; set; }

        public virtual WebsiteUser WebsiteUser { get; set; }
    }
}
