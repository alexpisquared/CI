using CI.GUI.Support.WpfLibrary.Base;
using CI.GUI.Support.WpfLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CI.GUI.Support.WpfLibrary.Views
{
  public partial class ExceptionPopup : WindowBase
  {
    public ExceptionPopup() => InitializeComponent();
    public ExceptionPopup(Exception ex, string optl, string cmn, string cfp, int cln) : this()
    {
      Loaded += (s, e) =>
      {
        ExType.Text = ex?.GetType().Name;
        callerFL.Text = $"{cfp} ({cln}): ";
        methodNm.Text = $"{cmn}()";
        optnlMsg.Text = optl;
        innrMsgs.Text = $"{ex.InnerMessages()}";
      };
    }

    async void onLoaded(object s, RoutedEventArgs e) { await Task.Delay(60111).ConfigureAwait(false); onCloseAndContinueExecution(s, e); }
    void onCloseAndContinueExecution(object s, RoutedEventArgs e) => Close(); // close popup and continue app execution
    void onShutdown(object s, RoutedEventArgs e) => Application.Current.Shutdown(55);
    void T4_MouseDown(object s, System.Windows.Input.MouseButtonEventArgs e) => onCopyClose(s, e);
    void onCopyClose(object s, RoutedEventArgs e)
    {
      Clipboard.SetText($"{callerFL.Text}\r\n{methodNm.Text}\r\n{optnlMsg.Text}\r\n{innrMsgs.Text}");
      onCloseAndContinueExecution(s, e);
    }
  }
}
