using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class AppStng
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Note { get; set; }
        public int LesnTyp { get; set; }
        public string SubLesnId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool ProLtgl { get; set; }
        public bool Audible { get; set; }
    }
}
