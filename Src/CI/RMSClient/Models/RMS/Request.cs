using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class Request
    {
        public int RequestId { get; set; }
        public int? CreatorId { get; set; }
        public int? ClientRequestId { get; set; }
        public int? SourceId { get; set; }
        public int? AccountId { get; set; }
        public int? TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public int? ActionId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string SecAdpnumber { get; set; }
        public string Cusip { get; set; }
        public string Symbol { get; set; }
        public string CurrencyCode { get; set; }
        public int? Quantity { get; set; }
        public int? DoneQty { get; set; }
        public double? Price { get; set; }
        public string OtherInfo { get; set; }
        public string Bbsnote { get; set; }
        public int? UserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string QuantityType { get; set; }
        public string CompoundFreq { get; set; }
        public string PaymentFreq { get; set; }
        public string Term { get; set; }
        public string Rate { get; set; }
    }
}
