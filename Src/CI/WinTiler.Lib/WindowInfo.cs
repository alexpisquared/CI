using System;
using System.Linq;

namespace WinTiler.Lib
{
  public class WindowInfo
  {
    public string ExePth { get; set; }
    public string WTitle { get; set; }
    public string Sorter { get; set; }
    public IntPtr Handle { get; set; }

    public WindowInfo(string title, string exePth, IntPtr handle)
    {
      WTitle = title;
      ExePth = exePth;
      Handle = handle;
      var appName = title.Split(" - ");
      Sorter = appName.Length > 1 ? $"{appName.LastOrDefault()} · {appName.FirstOrDefault()}" : $"{ExePth} · {WTitle}";
    }

    public override string ToString() => Sorter;
  }
}
