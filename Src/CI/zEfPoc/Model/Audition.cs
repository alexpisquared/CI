using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class Audition
    {
        public int Id { get; set; }
        public string PlayerAnswer { get; set; }
        public int ReReadCount { get; set; }
        public double TimeToAnswerSec { get; set; }
        public bool IsCorrect { get; set; }
        public bool? IsBadSaying { get; set; }
        public DateTime DoneAt { get; set; }
        public string DoneBy { get; set; }
        public int ProblemId { get; set; }
        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
        public virtual Problem Problem { get; set; }
    }
}
