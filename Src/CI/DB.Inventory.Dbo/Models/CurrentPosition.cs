using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class CurrentPosition
    {
        public string Usymbol { get; set; }
        public int BookId { get; set; }
        public DateTime UpdateDate { get; set; }
        public double ProfitLoss { get; set; }
        public int QuantityOutstanding { get; set; }
        public double AveragePrice { get; set; }
        public double AveragePriceJ { get; set; }
        public double TotalCommissions { get; set; }
        public double ConversionRate { get; set; }
        public double Mark { get; set; }
        public double? YesterdayBorrow { get; set; }
        public int YesterdaySharesOutstanding { get; set; }
        public double YesterdayAveragePrice { get; set; }
        public double YesterdayCommissions { get; set; }
        public double YesterdayMark { get; set; }
        public double? BidPrice { get; set; }
        public double? AskPrice { get; set; }
        public double? MidPrice { get; set; }

        public virtual Book Book { get; set; }
    }
}
