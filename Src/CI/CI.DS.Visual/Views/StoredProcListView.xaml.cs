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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CI.DS.Visual.Views
{
  public partial class StoredProcListView : UserControl
  {
    public StoredProcListView()
    {
      InitializeComponent();
    }

    void onLoaded(object sender, RoutedEventArgs e)
    {
      tbxSearch.Focus();
    }
  }
}
