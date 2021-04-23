using CI.DS.ViewModel;
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

namespace CI.DS.Visual.Views
{
  public partial class MainView : Window
  {
    public MainView(Microsoft.Extensions.Logging.ILogger<MainView> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent(); ;

      DataContext = new MainVM(_logger, _config);
    }
    void onExit(object s, RoutedEventArgs e) { Close(); ; }
  }
}
