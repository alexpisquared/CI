using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Tr
    {
        public string Account { get; set; }
        public string Mkt { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public string Side { get; set; }
        public string Tb { get; set; }
        public string Symbol { get; set; }
        public string Cusip { get; set; }
        public int Td { get; set; }
        public int? Sd { get; set; }
        public double Comm { get; set; }
        public string OpenClose { get; set; }
        public DateTime InsertTime { get; set; }
        public double? Fx { get; set; }
        public int Id { get; set; }
    }
}
