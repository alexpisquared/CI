namespace CI.DS.ViewModel
{
  public class StoredProcDetail
  {
    string sPName = "";

    public StoredProcDetail(string n, string p, string d)
    {
      (SPName, Parameters, Definition, Schema) = (n, p, d, "dbo");
    }

    public string SPName { get => sPName; private set => UFName = generateUserFriendlyName(sPName = value); }
    public string Schema { get; set; }
    public string Parameters { get; set; }
    public string Definition { get; set; }
    public string UFName { get; set; } = "";

    string generateUserFriendlyName(string val) => val.ToSentence();

    public override string ToString() { return $"{Schema}.{SPName}\r\n  ({Parameters})"; }
  }
}