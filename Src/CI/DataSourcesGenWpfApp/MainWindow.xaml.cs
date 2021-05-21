using System.Windows;

namespace DataSourcesGenWpfApp
{
  public partial class MainWindow : Window
  {
    public MainWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var inventoryDataSet = ((DataSourcesGenWpfApp.InventoryDataSet)(FindResource("inventoryDataSet")));

      var inventoryDataSetDBProcessTableAdapter = new DataSourcesGenWpfApp.InventoryDataSetTableAdapters.DBProcessTableAdapter();
      inventoryDataSetDBProcessTableAdapter.Fill(inventoryDataSet.DBProcess);
      var dBProcessViewSource = ((System.Windows.Data.CollectionViewSource)(FindResource("dBProcessViewSource")));
      dBProcessViewSource.View.MoveCurrentToFirst();

      var inventoryDataSetDBProcessParameterTableAdapter = new DataSourcesGenWpfApp.InventoryDataSetTableAdapters.DBProcessParameterTableAdapter();
      inventoryDataSetDBProcessParameterTableAdapter.Fill(inventoryDataSet.DBProcessParameter);
      var dBProcessDBProcessParameterViewSource = ((System.Windows.Data.CollectionViewSource)(FindResource("dBProcessDBProcessParameterViewSource")));
      dBProcessDBProcessParameterViewSource.View.MoveCurrentToFirst();

      var inventoryDataSetDBProcess_UserAccessTableAdapter = new DataSourcesGenWpfApp.InventoryDataSetTableAdapters.DBProcess_UserAccessTableAdapter();
      inventoryDataSetDBProcess_UserAccessTableAdapter.Fill(inventoryDataSet.DBProcess_UserAccess);
      var dBProcess_UserAccessViewSource = ((System.Windows.Data.CollectionViewSource)(FindResource("dBProcess_UserAccessViewSource")));
      dBProcess_UserAccessViewSource.View.MoveCurrentToFirst();
    }

    void Button_Click(object sender, RoutedEventArgs e) => Close();
  }
}
