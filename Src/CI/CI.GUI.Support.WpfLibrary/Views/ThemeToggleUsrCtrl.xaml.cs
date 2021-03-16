using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CI.GUI.Support.WpfLibrary.Views
{
  public partial class ThemeToggleUsrCtrl : UserControl
  {
    public ThemeToggleUsrCtrl() => InitializeComponent();
    public static readonly DependencyProperty CurThemeProperty = DependencyProperty.Register("CurTheme", typeof(string), typeof(ThemeToggleUsrCtrl), new PropertyMetadata(false)); public string CurTheme { get => (string)GetValue(CurThemeProperty); set => SetValue(CurThemeProperty, value); }

    void ToggleButton_Click(object s, RoutedEventArgs e) => ApplyTheme?.Invoke(CurTheme = ((ToggleButton)s).IsChecked == false ? "Lite.Bootstrap" : "Lite.Gray");
    public void SetCurThemeToMenu(string thm) => tgl1.IsChecked = thm == "Lite.Gray";

    public delegate void ApplyThemeDelegate(string v);
    public ApplyThemeDelegate? ApplyTheme { get; set; }

    void onChangeTheme(object s, RoutedEventArgs e) => ApplyTheme?.Invoke(((Button)s)?.Tag?.ToString() ?? "No Theme");
  }
}
