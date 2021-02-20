using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class RmsDboRequestBrDboAccountView
    {
        public long AccountId { get; set; }
        public int OrderId { get; set; }
        public string Account { get; set; }
        public string AdpAcountNumber { get; set; }
        public string AccountNum { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public string SecAdpnumber { get; set; }
        public string Cusip { get; set; }
        public string Symbol { get; set; }
        public string CurrencyCode { get; set; }
        public int? Quantity { get; set; }
        public int? DoneQty { get; set; }
        public double? Price { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string CompoundFreq { get; set; }
        public string PaymentFreq { get; set; }
        public string Term { get; set; }
        public string Rate { get; set; }
        public string OtherInfo { get; set; }
    }
}
