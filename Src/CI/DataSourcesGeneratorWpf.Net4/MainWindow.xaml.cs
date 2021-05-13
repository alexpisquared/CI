using System.Windows;

namespace DataSourcesGeneratorWpf.Net4
{
  public partial class MainWindow : Window
  {
    public MainWindow() { InitializeComponent(); }

    void Window_Loaded(object sender, RoutedEventArgs e)
    {
      TradeDataDataSet tradeDataDataSet = ((TradeDataDataSet)(FindResource("tradeDataDataSet")));

      TradeDataDataSetTableAdapters.OrderHistoryCIDTableAdapter tradeDataDataSetOrderHistoryCIDTableAdapter = new DataSourcesGeneratorWpf.Net4.TradeDataDataSetTableAdapters.OrderHistoryCIDTableAdapter();
      tradeDataDataSetOrderHistoryCIDTableAdapter.Fill(tradeDataDataSet.OrderHistoryCID);
      System.Windows.Data.CollectionViewSource orderHistoryCIDViewSource = ((System.Windows.Data.CollectionViewSource)(FindResource("orderHistoryCIDViewSource")));
      orderHistoryCIDViewSource.View.MoveCurrentToFirst();
    }

    void onClose(object sender, RoutedEventArgs e) { Close(); }
  }
}
