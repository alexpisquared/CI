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

namespace RMSClient
{
  public partial class RmsClientMainWindow : Window
  {
    readonly BRContext _dbBR = new BRContext();
    readonly RMSContext _dbRM = new RMSContext();
    CollectionViewSource categoryViewSource;

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
    }

    async void Window_Loaded(object sender, RoutedEventArgs e) => await find();

    async Task find()
    {
      var sw = Stopwatch.StartNew();
      //_db.Database.EnsureCreated();
      _dbRM.Requests.Load();
      _dbRM.RequestTypes.Load();
      _dbRM.SubTypes.Load();
      _dbRM.Sources.Load();
      _dbRM.Statuses.Load();
      categoryViewSource.Source = _dbRM.Requests.Local.ToObservableCollection();

      const int top = 12;
      var report = "";
      try
      {
        var fullrv = _dbRM.Requests.Where(r => dt1.SelectedDate <= r.CreationDate && r.CreationDate <= dt2.SelectedDate);
        report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";

        //categoryViewSource.Source =         DataContext = await fullrv.Take(top).ToListAsync();

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
