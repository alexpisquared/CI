namespace CI.DS.ViewModel
{
  public class SpdAdm : StoredProcDetail { public SpdAdm(string db, string s, string n, string p, string d, int e) : base(db, s, n, p, d, e) { } }
  public class SpdUsr : StoredProcDetail { public SpdUsr(string db, string s, string n, string p, string d, int e) : base(db, s, n, p, d, e) { } }
  public class SpdFbk : StoredProcDetail { public SpdFbk(string db, string s, string n, string p, string d, int e) : base(db, s, n, p, d, e) { } }
  public abstract class StoredProcDetail
  {
    string _spName = "";

    public StoredProcDetail(string db, string s, string n, string p, string d, int e) => (DBName, Schema, SPName, Parameters, Definition, HasExecPerm) = (db, s, n, p, d, e);

    public string Schema { get; set; }
    public string SPName { get => _spName; private set => UFName = generateUserFriendlyName(_spName = value); }
    public string Parameters { get; set; } // CSV!
    public string Definition { get; set; }
    public int HasExecPerm { get; set; }
    public string UFName { get; set; } = "";
    public string DBName { get; set; } = "Inventory";

    string generateUserFriendlyName(string val) => val.ToSentence();

    public override string ToString() => $"{Schema}.{SPName}\r\n  ({Parameters})";
  }
}