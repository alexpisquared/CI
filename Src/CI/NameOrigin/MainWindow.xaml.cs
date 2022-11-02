using System.Windows;
using NameOrigin.EF4;
namespace NameOrigin;
public partial class MainWindow : Window
{
  public MainWindow() => InitializeComponent();

  private void Button_Click(object sender, RoutedEventArgs e) => Program.Main__();
}
