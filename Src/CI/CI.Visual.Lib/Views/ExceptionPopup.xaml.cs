using CI.Visual.Lib.Base;
using CI.Standard.Lib.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CI.Visual.Lib.Views
{
  public partial class ExceptionPopup : WindowBase
  {
    public ExceptionPopup() => InitializeComponent();
    public ExceptionPopup(Exception ex, string optl, string cmn, string cfp, int cln, Window? owner) : this()
    {
      IgnoreWindowPlacement = true;
      Owner = owner;
      WindowStartupLocation = owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
      InitializeComponent();

      Loaded += (s, e) =>
      {
        ExType.Text = ex?.GetType().Name;
        callerFL.Text = $"{cfp} ({cln}): ";
        methodNm.Text = $"{cmn}()";
        optnlMsg.Text = optl;
        innrMsgs.Text = $"{ex.InnerMessages()}";
      };
    }

    async void onLoaded(object s, RoutedEventArgs e)
    {
      await Helpers.Bpr.Error();
      await Task.Delay(60111).ConfigureAwait(false); 
      onIgnoreIt(s, e);
    }
    void onIgnoreIt(object s, RoutedEventArgs e) => Close(); // close popup and continue app execution
    void onShutdown(object s, RoutedEventArgs e) => Application.Current.Shutdown(55);
    void T4_MouseDown(object s, System.Windows.Input.MouseButtonEventArgs e) => onCopyClose(s, e);
    void onCopyClose(object s, RoutedEventArgs e)
    {
      Clipboard.SetText($"{callerFL.Text}\r\n{methodNm.Text}\r\n{optnlMsg.Text}\r\n{innrMsgs.Text}");
      onIgnoreIt(s, e);
    }
  }
}
