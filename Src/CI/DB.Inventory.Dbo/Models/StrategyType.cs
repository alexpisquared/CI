using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class StrategyType
    {
        public StrategyType()
        {
            Books = new HashSet<Book>();
        }

        public int Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
