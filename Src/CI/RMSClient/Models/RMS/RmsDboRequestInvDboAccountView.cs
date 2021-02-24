using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RMSClient.Models.RMS
{
    public partial class RmsDboRequestInvDboAccountView
    {
    [Key] //tu: patch vw-no-kwy 2/2 
    public int OrderId { get; set; }
        public string ShortName { get; set; }
        public string AdpaccountCode { get; set; }
        public string RepCd { get; set; }
        public string TypeName { get; set; }
        public string SubtypeName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? SendingTimeGmt { get; set; }
        public int? AdpNumber { get; set; }
        public string Cusip { get; set; }
        public string Symbol { get; set; }
        public string Currency { get; set; }
        public string OrderQty { get; set; }
        public int? CumQty { get; set; }
        public double? AvgPx { get; set; }
        public string UserName { get; set; }
        public string OtherInfo { get; set; }
        public int? Quantity { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? UpdateTineGmt { get; set; }
        public int? ActionId { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public int? LeavesQty { get; set; }
    public int? AccountId { get; set; }
  }
}
