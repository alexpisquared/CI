using CI.Visual.Lib.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CI.DS.Visual.Views
{
  public partial class DBProcessListView : UserControl
  {
    public DBProcessListView()
    {
      InitializeComponent();
      tbxSearch.PreviewKeyDown += onUpDown;
    }

    async void onUpDown(object s, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Up: if (lbxSpds.SelectedIndex > 0) lbxSpds.SelectedIndex--; else await Bpr.No(); break;
        case Key.Down: if (lbxSpds.SelectedIndex < lbxSpds.Items.Count - 1) lbxSpds.SelectedIndex++; else await Bpr.No(); break;
        default: break;
      }
    }

    async void onLoaded(object s, RoutedEventArgs e)
    {
      tbxSearch.Focus();
      await Task.Delay(150);
      lbxSpds.SelectedIndex = 0;
    }
  }
}
