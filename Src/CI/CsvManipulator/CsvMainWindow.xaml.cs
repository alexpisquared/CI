using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CsvManipulator
{
  public partial class CsvMainWindow : Window
  {
#if DEBUG
    const int _delayMs = 5, _repeat = 3;
#else
    const int _delayMs = 125, _repeat = 6;
#endif

    public CsvMainWindow() => InitializeComponent();

    ICsvConverter _converter;
    int _jobMode = 0;

    void onExit(object s, RoutedEventArgs e) { Close(); App.Current.Shutdown(); }
    async void onLoaded(object s, RoutedEventArgs e)
    {
#if DEBUG
      if(Environment.MachineName == "RAZER1")
      {
        Top = 1700; Left = 1100;
      }

      await prepare(Environment.MachineName == "RAZER1" ? @"C:\temp\CI.csv" : @"C:\Users\alex.pigida\Downloads\BBSSecurities202102121_test_withcomma.csv");
      await convert();
#endif
      await Task.Yield();
    }
    async void onDrop(object s, DragEventArgs e)
    {
      string filenameIPresume;

      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        filenameIPresume = (string)e.Data.GetData(DataFormats.StringFormat);
        goto OK;
      }

      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        tbxInfo.Text = $"Must a file which is dropped.";
        return;
      }

      var fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
      if (fileNames?.Length < 1)
      {
        tbxInfo.Text = $"{fileNames?.Length / 1024.0} kb is not enough.";
        return;
      }

      filenameIPresume = fileNames[0];

      OK:
      if (!File.Exists(filenameIPresume))
      {
        tbxInfo.Text = $"{filenameIPresume}\n\n\t appears to be unavailable";
        return;
      }

      if (System.IO.Path.GetExtension(filenameIPresume).ToLower() != ".csv")
      {
        tbxInfo.Text = $"{filenameIPresume}\n\n\t is not good\n\n\t\t must be a CSV-file";
        return;
      }

      await prepare(filenameIPresume);
    }
    async void onConvert(object s, RoutedEventArgs e) => await convert();
    void onMouseLeft(object s, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
    void onSetCsvToJson(object s, RoutedEventArgs e) { e.Handled = true; setUI(0); }
    void onSetGrouperBy(object s, RoutedEventArgs e) { e.Handled = true; setUI(1); }

    void setUI(int jobMode)
    {
      if (_jobMode == jobMode || mnuBth == null || mnuRow == null || mnuCol == null) return;

      _jobMode = jobMode;
      mnuBth.IsChecked = !(mnuCol.IsChecked = jobMode == 0);
    }
    async Task prepare(string filenameIPresume)
    {
      tbxDrop.Visibility = Visibility.Collapsed;

      _converter =
        _jobMode == 0 ? new CsvConverter(filenameIPresume) :
        _jobMode == 1 ? new CsvConverter(filenameIPresume) :
                   /**/ new CsvConverter(filenameIPresume);

      tbxInfo.Text = await _converter.GetFileStats();
      buttonD.Visibility = Visibility.Visible;
    }
    async Task convert()
    {
      imgTarget.Visibility =
      mnuPanel.Visibility =
      buttonD.Visibility = Visibility.Hidden;

      for (var i = 0; i < 3; i++)
      {
        tbxInfo.Text = $"Converting now ...\r\n\n\n\t\t So far so good ... "; await Task.Delay(_delayMs);
        tbxInfo.Text = $"Converting now ...\r\n\n\n\t\t So far so good .. ."; await Task.Delay(_delayMs);
        tbxInfo.Text = $"Converting now ...\r\n\n\n\t\t So far so good . .."; await Task.Delay(_delayMs);
        tbxInfo.Text = $"Converting now ...\r\n\n\n\t\t So far so good  ..."; await Task.Delay(_delayMs * 2);
      }

      tbxInfo.Text = _converter.CleanEmptyRowsColumns();
    }
  }
}