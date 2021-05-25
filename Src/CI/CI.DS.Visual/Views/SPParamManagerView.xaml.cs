using System.Windows;
using System.Windows.Controls;

namespace CI.DS.Visual.Views
{
  public partial class SPParamManagerView : UserControl
  {
    public SPParamManagerView() => InitializeComponent();

    void onLoaded(object s, RoutedEventArgs e) => focus0.Focus();
  }
}