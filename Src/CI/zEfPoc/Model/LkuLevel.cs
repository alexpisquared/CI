﻿using System;
using System.Collections.Generic;

#nullable disable

namespace zEfPoc.Model
{
    public partial class LkuLevel
    {
        public LkuLevel()
        {
            Problems = new HashSet<Problem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public virtual ICollection<Problem> Problems { get; set; }
    }
}