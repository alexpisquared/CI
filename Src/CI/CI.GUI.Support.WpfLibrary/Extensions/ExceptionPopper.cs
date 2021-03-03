﻿using CI.GUI.Support.WpfLibrary.Views;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace CI.GUI.Support.WpfLibrary.Extensions
{
  public static class ExnPopr
  {
    public static void Pop(this Exception ex, Window? owner , string optl = "", [CallerMemberName] string cmn = "", [CallerFilePath] string cfp = "", [CallerLineNumber] int cln = 0)
    {
      var msgForPopup = ex.Log(optl);

      if (!Debugger.IsAttached)
      {
        try
        {
          var pop = new ExceptionPopup(ex, optl, cmn, cfp, cln, owner);
          WpfUtils.AutoInvokeOnUiThread(pop.ShowDialog);
        }
        catch (Exception ex2)
        {
          Trace.WriteLine(ex2.Message, MethodBase.GetCurrentMethod()?.Name);
          if (Debugger.IsAttached)
            Debugger.Break();
          else
            MessageBox.Show($"{cfp}({cln}): {cmn}()\r\n\n{(optl?.Length > 0 ? optl + "\r\n" : "")}\n{ex.InnerMessages()}", $"Exception During Exception in {cmn}", MessageBoxButton.OK, MessageBoxImage.Error);

          throw;
        }
      }
    }
  }
}
