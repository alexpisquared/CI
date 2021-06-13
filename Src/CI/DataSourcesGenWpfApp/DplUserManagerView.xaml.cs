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

namespace DataSourcesGenWpfApp
{
  public partial class DplUserManagerView : Window
  {
    public DplUserManagerView()
    {
      InitializeComponent();
    }
    void onClose(object s, RoutedEventArgs e) => Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DataSourcesGenWpfApp.DplUserManagerData dplUserManagerData = ((DataSourcesGenWpfApp.DplUserManagerData)(this.FindResource("dplUserManagerData")));

      // Load data into the table User. You can modify this code as needed.
      DataSourcesGenWpfApp.DplUserManagerDataTableAdapters.UserTableAdapter dplUserManagerDataUserTableAdapter = new DataSourcesGenWpfApp.DplUserManagerDataTableAdapters.UserTableAdapter();
      dplUserManagerDataUserTableAdapter.Fill(dplUserManagerData.User);
      System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
      userViewSource.View.MoveCurrentToFirst();

      // Load data into the table Role. You can modify this code as needed.
      DataSourcesGenWpfApp.DplUserManagerDataTableAdapters.RoleTableAdapter dplUserManagerDataRoleTableAdapter = new DataSourcesGenWpfApp.DplUserManagerDataTableAdapters.RoleTableAdapter();
      dplUserManagerDataRoleTableAdapter.Fill(dplUserManagerData.Role);
      System.Windows.Data.CollectionViewSource roleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("roleViewSource")));
      roleViewSource.View.MoveCurrentToFirst();
    }
  }
}
