using System.Windows;

namespace DataSourcesGeneratorWpf.Net4
{
  public partial class MainWindow : Window
  {
    public MainWindow() { InitializeComponent(); }

    void Window_Loaded(object sender, RoutedEventArgs e)
    {
      new TradeDataDataSetTableAdapters.OrderHistoryCIDTableAdapter().Fill(((TradeDataDataSet)(FindResource("tradeDataDataSet"))).OrderHistoryCID);

      ((System.Windows.Data.CollectionViewSource)(FindResource("orderHistoryCIDViewSource"))).View.MoveCurrentToFirst();
    }

    void onClose(object sender, RoutedEventArgs e) { Close(); }
  }
}
