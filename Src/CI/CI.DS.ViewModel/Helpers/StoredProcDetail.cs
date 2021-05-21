namespace CI.DS.ViewModel
{
  public class StoredProcDetail
  {
    string _spName = "";

    public StoredProcDetail(string s, string n, string p, string d, int e) => (Schema, SPName, Parameters, Definition, HasExecPerm) = (s, n, p, d, e);

    public string Schema { get; set; }
    public string SPName { get => _spName; private set => UFName = generateUserFriendlyName(_spName = value); }
    public string Parameters { get; set; } // CSV!
    public string Definition { get; set; }
    public int HasExecPerm { get; set; }
    public string UFName { get; set; } = "";

    string generateUserFriendlyName(string val) => val.ToSentence();

    public override string ToString() => $"{Schema}.{SPName}\r\n  ({Parameters})";
  }
}