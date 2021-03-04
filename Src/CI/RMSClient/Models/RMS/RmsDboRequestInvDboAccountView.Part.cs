#nullable disable

namespace RMSClient.Models.RMS
{
  public partial class RmsDboRequestInvDboAccountView
  {
    public override string ToString() =>
      $"{ShortName}\t{AdpaccountCode}\t{RepCd}\t{TypeName}\t{SubtypeName}\t{OrderStatus}\t{SendingTimeGmt}\t{AdpNumber}\t{Cusip}\t{Symbol}\t{Currency}\t{OrderQty}\t{CumQty}\t{AvgPx}\t{UserName}\t{OtherInfo}\t{Quantity}\t{UserId}\t{StatusId}\t{UpdateTineGmt}\t{ActionId}\t{Action}\t{Note}\t{AccountId}\t{LeavesQty}\t{SecAdpnumber}\t{CurrencyCode}\t{CompoundFreq}\t{PaymentFreq}\t{Term}\t{Rate}";
  }
}