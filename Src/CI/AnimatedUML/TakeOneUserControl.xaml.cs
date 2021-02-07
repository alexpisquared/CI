using System.Windows;
using System.Windows.Controls;

namespace AnimatedUML
{
  public partial class TakeOneUserControl : UserControl
  {
    public TakeOneUserControl() => InitializeComponent();

    void onCLose(object sender, RoutedEventArgs e) => App.Current.Shutdown(); // ~~ Close();
  }
}
