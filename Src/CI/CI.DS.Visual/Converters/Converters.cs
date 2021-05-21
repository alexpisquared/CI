using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CI.DS.Visual.Converters
{
  public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (((bool?)value) == null ? Visibility.Collapsed : ((bool?)value) == true ? Visibility.Visible : Visibility.Collapsed);
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => false;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
    public BoolToVisibilityConverter() { }
  }

}
