using CI.Visual.Lib.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CI.DS.Visual.Views
{
  public partial class StoredProcListView : UserControl
  {
    public StoredProcListView()
    {
      InitializeComponent();

      tbxSpdSearch.PreviewKeyDown += async (s, e) =>
      {
        switch (e.Key)
        {
          case Key.Up: if (lbxSpds.SelectedIndex > 0) lbxSpds.SelectedIndex--; else await Bpr.No(); break;
          case Key.Down: if (lbxSpds.SelectedIndex < lbxSpds.Items.Count - 1) lbxSpds.SelectedIndex++; else await Bpr.No(); break;
          default: break;
        }
      };

      tbxDblSearch.PreviewKeyDown += async (s, e) =>
      {
        switch (e.Key)
        {
          case Key.Up: if (lbxDbls.SelectedIndex > 0) lbxDbls.SelectedIndex--; else await Bpr.No(); break;
          case Key.Down: if (lbxDbls.SelectedIndex < lbxDbls.Items.Count - 1) lbxDbls.SelectedIndex++; else await Bpr.No(); break;
          default: break;
        }
      };
    }

    async void onLoaded(object s, RoutedEventArgs e)
    {
      tbxSpdSearch.Focus();
      await Task.Delay(150);
      lbxDbls.SelectedIndex = 0;
      lbxSpds.SelectedIndex = 0;
    }

    void ToggleButton_Click(object sender, RoutedEventArgs e)    {    }
  }
}
