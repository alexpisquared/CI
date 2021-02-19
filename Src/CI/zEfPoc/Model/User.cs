using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class User
    {
        public User()
        {
            SessionResults = new HashSet<SessionResult>();
        }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<SessionResult> SessionResults { get; set; }
    }
}
