using System;
using System.Windows;
using System.Windows.Controls;

namespace CI.Standard.Lib.Views
{
  public partial class ThemeSelectorUsrCtrl : UserControl
  {
    public delegate void ApplyThemeDelegate(string v);
    public ThemeSelectorUsrCtrl() => InitializeComponent();
    public ApplyThemeDelegate? ThemeApplier { get; set; }
    public static readonly DependencyProperty CurThemeProperty = DependencyProperty.Register("CurTheme", typeof(string), typeof(ThemeSelectorUsrCtrl)); public string CurTheme { get => (string)GetValue(CurThemeProperty); set => SetValue(CurThemeProperty, value); }
    public void SetCurThemeToMenu(string theme)
    {
      foreach (MenuItem? item in ((ItemsControl)menu1.Items[0]).Items)
        if (item != null)
          item.IsChecked = theme?.Equals(item.Tag.ToString(), StringComparison.OrdinalIgnoreCase) ?? false;
    }

    void onChangeTheme(object s, RoutedEventArgs e) => ThemeApplier?.Invoke(((Button)s)?.Tag?.ToString() ?? "No Theme");
    void onSelectionChanged(object s, SelectionChangedEventArgs e)
    {
      if (e?.AddedItems?.Count > 0 && ThemeApplier != null)
        ThemeApplier(CurTheme = ((FrameworkElement)((object[])e.AddedItems)[0]).Tag?.ToString() ?? "No Theme?");
    }
    void onMenuClick(object s, RoutedEventArgs e)
    {
      ThemeApplier?.Invoke(CurTheme = ((FrameworkElement)s).Tag?.ToString() ?? "No Theme");

      SetCurThemeToMenu(CurTheme);
    }
  }
}
