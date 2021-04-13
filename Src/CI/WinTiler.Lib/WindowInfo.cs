using System;
using System.Linq;

namespace WinTiler.Lib
{
  public class WindowInfo
  {
    public string WTitle { get; set; }
    public string Sorter { get; set; }
    public IntPtr Handle { get; set; }

    public WindowInfo(string title, IntPtr handle)
    {
      WTitle = title;
      Handle = handle;
      var appName = title.Split(" - ");
      Sorter = appName.Length > 1 ? $"{appName.LastOrDefault()} · {appName.FirstOrDefault()}" : $"· {WTitle}";
    }

    public override string ToString() => Sorter;
  }
}
