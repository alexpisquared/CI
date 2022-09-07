using System;

namespace WinTiler.Lib
{
  public class WindowInfo
  {
    public string AppNme { get; set; }
    public string WTitle { get; set; }
    public string Sorter { get; set; }
    public IntPtr Handle { get; set; }

    public WindowInfo(string title, string exePth, IntPtr handle)
    {
      WTitle = title;
      AppNme = exePth;
      Handle = handle;
      _ = title.Split(" - ");
      Sorter = // appName.Length > 1 ? $"{appName.LastOrDefault()} · {appName.FirstOrDefault()}" : 
        $"{AppNme} · {WTitle}";
    }

    public override string ToString() => Sorter;
  }
}
