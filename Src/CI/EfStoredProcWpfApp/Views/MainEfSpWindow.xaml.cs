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

namespace EfStoredProcWpfApp.Views
{
  public partial class MainEfSpWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    public MainEfSpWindow(Microsoft.Extensions.Logging.ILogger<MainEfSpWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot? _config)
    {
      InitializeComponent();
    }
  }
}
