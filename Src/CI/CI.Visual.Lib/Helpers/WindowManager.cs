using System;
using System.Diagnostics;
using System.Linq;

namespace CI.Visual.Lib.Helpers
{
  public class WindowManager
  {
    public void Arrange() { listProcesses(); ; }

    void listProcesses()
    {
      var i = 0;
      var processCollection = Process.GetProcesses(".").Where(p => p.MainWindowHandle != IntPtr.Zero
        //&& p.ProcessName != "explorer"
        ).OrderBy(p => p.ProcessName);
      Debug.WriteLine($"{processCollection.Count(),3}  total:");

      foreach (var p in processCollection)
        Debug.WriteLine($"{++i,3}  {p.ProcessName,-22}  {p.MainWindowTitle}");

      Debug.WriteLine($"{processCollection.Count(),3}  total:");
    }
  }
}
