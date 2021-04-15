using System.Collections.Generic;

namespace WinTiler.Lib
{
  public class UserPrefs
  {
    List<string> _exestoIgnore = new();

    public List<string> ExesToIgnore { get => _exestoIgnore; set => _exestoIgnore = value; }
    public List<string> TitlToIgnore { get; set; } = new();
    public bool SkipMinimized { get; set; }
  }
}
