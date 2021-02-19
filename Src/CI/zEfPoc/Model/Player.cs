using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class Player
    {
        public Player()
        {
            Auditions = new HashSet<Audition>();
            TombStones = new HashSet<TombStone>();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Desc { get; set; }
        public DateTime AddedAt { get; set; }
        public string AddedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<Audition> Auditions { get; set; }
        public virtual ICollection<TombStone> TombStones { get; set; }
    }
}
