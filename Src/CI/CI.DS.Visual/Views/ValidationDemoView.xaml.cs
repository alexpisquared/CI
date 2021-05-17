using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CI.DS.Visual.Views
{
  public partial class ValidationDemoView : UserControl
  {
    public ValidationDemoView() => InitializeComponent();
    async void onLoaded(object s, RoutedEventArgs e) { await Task.Delay(99); focus0.Focus(); }
  }
}
