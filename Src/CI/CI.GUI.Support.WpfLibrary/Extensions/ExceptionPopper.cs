using CI.GUI.Support.WpfLibrary.Views;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CI.GUI.Support.WpfLibrary.Extensions
{
    public static class ExnPopr
    {
        public static void Pop(this Exception ex, Window? owner, string optl = "", [CallerMemberName] string cmn = "", [CallerFilePath] string cfp = "", [CallerLineNumber] int cln = 0)
        {
            var msgForPopup = ex.Log(optl);

            if (Debugger.IsAttached)
                return;

            try
            {
                WpfUtils.AutoInvokeOnUiThread(new ExceptionPopup(ex, optl, cmn, cfp, cln, owner).ShowDialog);
            }
            catch (Exception ex2)
            {
                Trace.WriteLine(ex2.Message, MethodBase.GetCurrentMethod()?.Name);
                if (Debugger.IsAttached)
                    Debugger.Break();
                else
                    MessageBox.Show($"{ex2.InnerMessages()} \n\nwhile reporting this:\n\n{msgForPopup}", $"Exception During Exception in {cmn}", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }
        }
    }
}