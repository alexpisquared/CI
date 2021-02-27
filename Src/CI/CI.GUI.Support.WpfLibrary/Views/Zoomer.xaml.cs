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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CI.GUI.Support.WpfLibrary.Views
{
  /// <summary>
  /// Interaction logic for Zoomer.xaml
  /// </summary>
  public partial class Zoomer : UserControl
  {
    public Zoomer()
    {
      InitializeComponent();

			//Loaded += (_slct, e) =>			{				try { ZoomSlider.ZmValue = Settings.Default.Zoom; }				catch { ZoomSlider.ZmValue = 1; }			};

			ZoomSlider.MouseDoubleClick += (s, e) => ZoomSlider.Value = 1;
			ZoomPrgBar.MouseDoubleClick += (s, e) => ZoomSlider.Value = 1;
			ZoomSlider.MouseWheel += (s, e) => ZoomSlider.Value += (e.Delta * .0002);
			ZoomPrgBar.MouseWheel += (s, e) => ZoomSlider.Value += (e.Delta * .0002);

			DataContext = this;
		}

		public static readonly DependencyProperty ZmValueProperty = DependencyProperty.Register("ZmValue", typeof(double), typeof(Zoomer), new PropertyMetadata(1.0));		public double ZmValue { get { return (double)GetValue(ZmValueProperty); } set { SetValue(ZmValueProperty, value); } }
	}

	public class Multy100 : MarkupExtension, IValueConverter
	{
		public Multy100() { }
		public bool InvertValue { get; set; } // public bool InvertValue		{			get { return _InvertValue; }			set { _InvertValue = value; }		}		private bool _InvertValue = false;

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is double)) return value;

			return 100.0 * (double)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) { throw new NotImplementedException(string.Format("Under construction: {0}.{1}()", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodInfo.GetCurrentMethod().Name)); ; }
		public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
	}
}