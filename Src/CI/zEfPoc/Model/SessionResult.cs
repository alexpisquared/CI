using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class SessionResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ExcerciseName { get; set; }
        public int PokedIn { get; set; }
        public DateTime DoneAt { get; set; }
        public TimeSpan Duration { get; set; }
        public string Note { get; set; }
        public bool? IsRecord { get; set; }
        public int? TotalRunCount { get; set; }

        public virtual User User { get; set; }
    }
}
