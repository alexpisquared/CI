using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class Problem
    {
        public Problem()
        {
            Auditions = new HashSet<Audition>();
        }

        public int Id { get; set; }
        public string ProblemText { get; set; }
        public string SolutionText { get; set; }
        public string HintMessage { get; set; }
        public string BatchSource { get; set; }
        public string Notes { get; set; }
        public int Grade { get; set; }
        public DateTime AddedAt { get; set; }
        public string AddedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public int? LevelId { get; set; }
        public int? LanguageId { get; set; }

        public virtual LkuLanguage Language { get; set; }
        public virtual LkuLevel Level { get; set; }
        public virtual ICollection<Audition> Auditions { get; set; }
    }
}
