using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CanadianAt
    {
        public string ExchangeCode { get; set; }
        public string ListedExchangeCode { get; set; }
        public string TicketExchangeCode { get; set; }
    }
}
