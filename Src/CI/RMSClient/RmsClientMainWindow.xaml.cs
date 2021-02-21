using Microsoft.EntityFrameworkCore;
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
    readonly RMSContext _dbRMS = new RMSContext();
    readonly CollectionViewSource categoryViewSource;
    bool _loaded = false;

    public RmsClientMainWindow()
    {
      InitializeComponent();//DataContext = this;

      dt1.SelectedDate = DateTimeOffset.Now.Date.AddYears(-100);
      dt2.SelectedDate = DateTimeOffset.Now.Date;

      categoryViewSource = (CollectionViewSource)FindResource(nameof(categoryViewSource));

#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1650; Left = 10; }
      else { Top = 1600; Left = 2500; }
#endif
      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZVa += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZVa:{ZVa}"); }; //tu:
    }

    public static readonly DependencyProperty ZVaProperty = DependencyProperty.Register("ZVa", typeof(double), typeof(RmsClientMainWindow), new PropertyMetadata(1.25)); public double ZVa { get => (double)GetValue(ZVaProperty); set => SetValue(ZVaProperty, value); }

    async void onLoaded(object sender, RoutedEventArgs e) { _loaded = true; await find(); } //_db.Database.EnsureCreated();
    async void onDiRein(object sender, RoutedEventArgs e) => await find();
    async void onDateCh(object sender, SelectionChangedEventArgs e) => await find();

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

#if DIRECT
        var fullrv = _dbRM.RmsDboRequestBrDboAccountViews.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate);
        var report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";
        DataContext = await fullrv.Take(top).ToListAsync();
#else
        await _dbRMS.RmsDboRequestBrDboAccountViews                             /**/.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate && (acnt == null || r.AdpAcountNumber.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv")))).LoadAsync();
        var l = _dbRMS.RmsDboRequestBrDboAccountViews.Local.ToObservableCollection().Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate && (acnt == null || r.AdpAcountNumber.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv"))));
        categoryViewSource.Source = l.Take(top);
        var report = top == l.Count() ?
          $"Total {l.Count()} matches found in " :
          $"Top {Math.Min(top, l.Count())} rows out of {l.Count()} matches found in ";
#endif

        Title = $"RMS Client ({Environment.UserName}) - {report} {sw.Elapsed.TotalSeconds:N2} sec.";
        Debug.WriteLine(sw.Elapsed);
        await Task.Delay(250);
      }
      catch (Exception ex) { Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
      finally
      {
        vb1.Visibility = Visibility.Collapsed;
        tbxAccount.Focus();
      }
    }
    async void onFind(object sender, RoutedEventArgs e) => await find();

    void onExit(object sender, RoutedEventArgs e) => Close();

    void onClip(object sender, RoutedEventArgs e)
    {
      try
      {
        Clipboard.SetData(DataFormats.StringFormat, DataContext);
      }
      catch (Exception ex) { Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      _dbRMS.Dispose();
      _dbRMS.Dispose();
      base.OnClosing(e);
    }

  }
}