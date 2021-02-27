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

namespace CI.GUI.Support.WpfLibrary.Views
{
  /// <summary>
  /// Interaction logic for ThemeSelectorUsrCtrl.xaml
  /// </summary>
  public partial class ThemeSelectorUsrCtrl : UserControl
  {
    public ThemeSelectorUsrCtrl() => InitializeComponent();

    public delegate void ApplyThemeDelegate(string v);
    public ApplyThemeDelegate? ApplyTheme { get; set; }

    public void SetCurTheme(string theme)
    {
      foreach (MenuItem? item in ((ItemsControl)menu1.Items[0]).Items)
        if (item != null)
          item.IsChecked = theme?.Equals(item.Tag.ToString(), StringComparison.OrdinalIgnoreCase) ?? false;
    }
    public static readonly DependencyProperty CurThemeProperty = DependencyProperty.Register("CurTheme", typeof(string), typeof(ThemeSelectorUsrCtrl), new PropertyMetadata(null)); public string CurTheme { get => (string)GetValue(CurThemeProperty); set => SetValue(CurThemeProperty, value); }

    void onChangeTheme(object s, RoutedEventArgs e) => ApplyTheme?.Invoke(((Button)s)?.Tag?.ToString() ?? "No Theme");
    void onSelectionChanged(object s, SelectionChangedEventArgs e)
    {
      if (e?.AddedItems?.Count > 0 && ApplyTheme != null)
        ApplyTheme(CurTheme = ((FrameworkElement)((object[])e.AddedItems)[0]).Tag?.ToString() ?? "No Theme?");
    }
    void onMenuClick(object s, RoutedEventArgs e)
    {
      ApplyTheme?.Invoke(CurTheme = ((FrameworkElement)s).Tag?.ToString() ?? "No Theme");

      SetCurTheme(CurTheme);
    }
  }
}
