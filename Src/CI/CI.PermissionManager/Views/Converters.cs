using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace CI.PermissionManager.Views
{
  public class StringToColor : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return value is string str && !string.IsNullOrEmpty(str) && !str.Equals("ColorRGB")
          ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(str))
          : Brushes.Teal;
      }
      catch
      {
        return Brushes.Red;
      }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => "";
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
    public StringToColor() { }
  }

  public class BoolToColorConverter : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new SolidColorBrush(((bool?)value) == null ? Colors.Transparent : ((bool?)value) == true ? Colors.Green : Colors.Red);
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => false;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
    public BoolToColorConverter() { }
  }
  public class BoolToCharConverter : MarkupExtension, IValueConverter // FontFamily="Wingdings" FontSize="26"
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((bool?)value) == null ? " " : ((bool?)value) == true ? "ü" : "û";
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => false;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
    public BoolToCharConverter() { }
  }
}