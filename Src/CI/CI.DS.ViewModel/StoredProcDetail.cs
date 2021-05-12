using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

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

    public override string ToString() { return SPName; }
  }
  public static class Extensions
  {
    public static string ToSentence1(this string val) => (new string(val.SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray())).Replace("usp_", "").Replace("sp_", "").Trim();
    public static string ToSentence2(this string val) => string.Concat(val.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ').Replace("usp_", "").Replace("sp_", "").Trim();
    public static string ToSentence(this string val) => Regex.Replace(val.Replace("usp_", "").Replace("sp_", ""), "([a-z])_?([A-Z])", "$1 $2").Replace("_", " ").Replace("_", " ").Trim();
    public static string ToSentence4(this string val) => Regex.Replace(val, "(?<!^)_?([A-Z])", " $1").Trim();
  }
}