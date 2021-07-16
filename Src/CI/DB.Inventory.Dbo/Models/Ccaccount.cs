using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Ccaccount
    {
        public int FileDateInt { get; set; }
        public string Rrcode { get; set; }
        public string AcctNum { get; set; }
        public string ClientNum { get; set; }
        public string AcctType { get; set; }
        public string AcctSubType { get; set; }
        public string AcctFund { get; set; }
        public string AcctShrtname { get; set; }
        public string AcctClass { get; set; }
        public string OptCode { get; set; }
        public string Debintcd { get; set; }
        public string Credintcd { get; set; }
        public string AcctSin { get; set; }
        public int? InitDate { get; set; }
        public int? LastUpdDate { get; set; }
        public int? CloseDate { get; set; }
        public string Beneficiary { get; set; }
        public string BenSin { get; set; }
        public string AcctLang { get; set; }
        public string BrnNum { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string Address7 { get; set; }
        public string Address8 { get; set; }
        public string PostCode { get; set; }
        public int? HoldMail { get; set; }
        public string ResCode { get; set; }
        public string NonResCode { get; set; }
        public int? BenBdate { get; set; }
        public string SpousalInd { get; set; }
        public string LockInInd { get; set; }
        public string SpouseJointName { get; set; }
        public string SpouseJointSin { get; set; }
        public int? SpouseJointBdate { get; set; }
        public string LockJourisdiction { get; set; }
        public string RecipientType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string StruckAdrInd { get; set; }
        public string Ni54a { get; set; }
        public string Ni54b { get; set; }
        public string Ni54c { get; set; }
        public string Ni54d { get; set; }
        public string TradesOk { get; set; }
        public string Approvedby { get; set; }
        public string Stat { get; set; }
        public string Employee { get; set; }
        public string CommType { get; set; }
        public string PhoneOther { get; set; }
        public string PhoneOther1 { get; set; }
        public string PhoneOther2 { get; set; }
        public string PhoneOther3 { get; set; }
        public string PortType { get; set; }
        public string EConfirm { get; set; }
        public string EStatement { get; set; }
        public string AddrNum { get; set; }
        public string AddrLvl { get; set; }
        public string InternalClientNumber { get; set; }
        public int? ClientOpenDate { get; set; }
        public long Id { get; set; }
    }
}
