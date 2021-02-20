using Microsoft.EntityFrameworkCore;
using RMSClient.Models.BR;
using RMSClient.Models.RMS;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace RMSClient
{
  public partial class RmsClientMainWindow : Window
  {
    readonly BRContext _dbBR = new BRContext();
    readonly RMSContext _dbRM = new RMSContext();
    readonly CollectionViewSource categoryViewSource;

    public RmsClientMainWindow()
    {
      InitializeComponent();//DataContext = this;

      dt1.SelectedDate = DateTimeOffset.Now.Date.AddYears(-1);
      dt2.SelectedDate = DateTimeOffset.Now.Date;

      categoryViewSource = (CollectionViewSource)FindResource(nameof(categoryViewSource));

#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1650; Left = 10; }
      else { Top = 1600; Left = 2500; }
#endif
      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZV += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZV:{ZV}"); }; //tu:
    }

    const double _defaultZoomV = 1.25;
    public static readonly DependencyProperty ZVProperty = DependencyProperty.Register("ZV", typeof(double), typeof(RmsClientMainWindow), new PropertyMetadata(_defaultZoomV)); public double ZV { get => (double)GetValue(ZVProperty); set => SetValue(ZVProperty, value); }

    async void Window_Loaded(object sender, RoutedEventArgs e) => await find(); //_db.Database.EnsureCreated();

    async Task find()
    {
      try
      {
        const int top = 12;
        var sw = Stopwatch.StartNew();

#if DIRECT
        var fullrv = _dbRM.Requests.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate);
        report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";
        DataContext = await fullrv.Take(top).ToListAsync();
#else
        await _dbRM.Requests.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate).LoadAsync();
        await _dbRM.RequestTypes.LoadAsync();
        await _dbRM.SubTypes.LoadAsync();
        await _dbRM.Sources.LoadAsync();
        await _dbRM.Statuses.LoadAsync();
        var fullrv = _dbRM.Requests.Local.ToObservableCollection().Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate);

        categoryViewSource.Source = fullrv;
        var report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";
#endif

        Title = $"RMS Client - {report} {sw.Elapsed.TotalSeconds:N2} sec.";
        Debug.WriteLine(sw.Elapsed);
        vb1.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex) { Clipboard.SetText(ex.Message); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
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
      _dbRM.Dispose();
      _dbRM.Dispose();
      base.OnClosing(e);
    }
  }
}
