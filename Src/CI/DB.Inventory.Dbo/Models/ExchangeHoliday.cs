using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class ExchangeHoliday
    {
        public string Location { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Description { get; set; }
        public int? IsExchangeHoliday { get; set; }
        public DateTime? ClosingTime { get; set; }
    }
}
