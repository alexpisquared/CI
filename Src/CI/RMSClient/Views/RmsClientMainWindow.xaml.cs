using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMSClient.Models.RMS;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RMSClient
{
  public partial class RmsClientMainWindow : Window
  {
    readonly ILogger<RmsClientMainWindow> _logger;
    readonly RMSContext _dbRMS = new RMSContext();
    readonly CollectionViewSource _accountRequestViewSource;
    bool _loaded = false;

    public RmsClientMainWindow(Microsoft.Extensions.Logging.ILogger<RmsClientMainWindow> logger)
    {
      InitializeComponent();//DataContext = this;

      dt1.SelectedDate = DateTimeOffset.Now.Date.AddYears(-100);
      dt2.SelectedDate = DateTimeOffset.Now.Date;

      _accountRequestViewSource = (CollectionViewSource)FindResource(nameof(_accountRequestViewSource));

#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1650; Left = 10; }
      else { Top = 1600; Left = 2500; }
#endif
      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZVa += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZVa:{ZVa}"); }; //tu:
      _logger = logger;
    }
    public static readonly DependencyProperty ZVaProperty = DependencyProperty.Register("ZVa", typeof(double), typeof(RmsClientMainWindow), new PropertyMetadata(1.25));

    public double ZVa { get => (double)GetValue(ZVaProperty); set => SetValue(ZVaProperty, value); }

    async void onLoaded(object sender, RoutedEventArgs e) { _loaded = true; await find(); } //_db.Database.EnsureCreated();
    async void onDiRein(object sender, RoutedEventArgs e) => await find();
    async void onDateCh(object sender, SelectionChangedEventArgs e) => await find();
    async void onFind(object sender, RoutedEventArgs e) => await find();
    async void onxAccountChanged(object s, TextChangedEventArgs e)
    {
      if (!_loaded) return;

      var prev = tbxAccount.Text;
      await Task.Delay(555);
      if (prev == tbxAccount.Text)
        await find();
    }

    async Task find()
    {
      if (!_loaded) return;

      try
      {
        btnFind.Focus();
        vb1.Visibility = Visibility.Visible;
        const int top = 12;
        var sw = Stopwatch.StartNew();
        var acnt = string.IsNullOrEmpty(tbxAccount.Text) || tbxAccount.Text == "xxxxxxxxx" ? null : tbxAccount.Text;

#if !DIRECT
        var l = _dbRMS.RmsDboRequestBrDboAccountViews.
          Where(r =>
          //r.TypeID != 5 &&
          dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate && (acnt == null || r.AdpAcountNumber.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv")))
          //&& (cbxOAF.SelectedValue.ToString() == "All" || (r.Status == cbxOAF.SelectedValue.ToString()))       
        );
        dg1.ItemsSource = await l.Take(top).ToListAsync();
#elif RawSql
        var fullrv = _dbRM.RmsDboRequestBrDboAccountViews.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate);
        var report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";
        DataContext = await fullrv.Take(top).ToListAsync();
#else
        await _dbRMS.RmsDboRequestBrDboAccountViews                             /**/.Where(r => /*r.TypeID!=5 &&*/ dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate && (acnt == null || r.AdpAcountNumber.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv"))) && (cbxOAF.SelectedValue.ToString() == "All" || (r.Status == cbxOAF.SelectedValue.ToString()))).          LoadAsync();
        var l = _dbRMS.RmsDboRequestBrDboAccountViews.Local.ToObservableCollection().Where(r => /*r.TypeID!=5 &&*/ dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate && (acnt == null || r.AdpAcountNumber.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv"))) && (cbxOAF.SelectedValue.ToString() == "All" || (r.Status == cbxOAF.SelectedValue.ToString())));
        _accountRequestViewSource.Source = l.Take(top);
#endif
        var report = l.Count() <= top ?
          $"Total {l.Count()} matches found in " :
          $"Top {Math.Min(top, l.Count()),3}  rows out of {l.Count(),5}  matches found in ";

        Title = $"RMS Client ({Environment.UserName}) - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";

        _logger.LogInformation($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {Title}   params: {dt1.SelectedDate} - {dt2.SelectedDate}   {acnt}   {(cnkDirein.IsChecked == true ? "Direct Reinvest" : "")}");

        await Task.Delay(333);
      }
      catch (Exception ex)
      {
        _logger.LogError($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {ex}");
        Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception 3 ", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        vb1.Visibility = Visibility.Collapsed;
        tbxAccount.Focus();
      }
    }
    async void dg1_SelectedCellsChanged(object s, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded) return;

      try
      {
        const int top = 12;
        var sw = Stopwatch.StartNew();
        var requestID = ((RmsDboRequestBrDboAccountView)((System.Windows.Controls.Primitives.Selector)s).SelectedValue).OrderId;

        var l = _dbRMS.RequestHistories.Where(r => r.RequestId == requestID);
        dg2.ItemsSource = await l.Take(top).ToListAsync();

        var report = l.Count() <= top ?
          $"Total {l.Count()} matches found in " :
          $"Top {Math.Min(top, l.Count()),3}  rows out of {l.Count(),5}  matches found in ";

        Title = $"RMS Client ({Environment.UserName}) - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";

        _logger.LogInformation($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {Title}   ");

        await Task.Delay(333);
      }
      catch (Exception ex)
      {
        _logger.LogError($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {ex}");
        Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception 3 ", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    void onClip(object sender, RoutedEventArgs e)
    {
      try
      {
        Clipboard.SetData(DataFormats.StringFormat, DataContext);
      }
      catch (Exception ex)
      {
        _logger.LogError($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {ex}");
        Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception 4 ", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
    void onExit(object sender, RoutedEventArgs e) => Close();

    protected override void OnClosing(CancelEventArgs e)
    {
      _dbRMS.Dispose();
      _dbRMS.Dispose();
      base.OnClosing(e);
    }
  }
}