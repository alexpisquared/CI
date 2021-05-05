﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CI.Standard.Lib.Views
{
  public partial class Zoomer : UserControl
  {
    public Zoomer()
    {
      InitializeComponent();

      //Loaded += (_slct, e) =>			{				try { ZoomSlider.ZmValue = Settings.Default.Zoom; }				catch { ZoomSlider.ZmValue = 1; }			};

      MouseDoubleClick += (s, e) => ZoomSlider.Value = 1;
      MouseWheel += (s, e) => ZoomSlider.Value += (e.Delta * .0002);
      //ZoomPrgBar.MouseDoubleClick += (s, e) => ZoomSlider.Value = 1;
      //ZoomPrgBar.MouseWheel += (s, e) => ZoomSlider.Value += (e.Delta * .0002);

      DataContext = this;
    }
    public static readonly DependencyProperty ZmValueProperty = DependencyProperty.Register("ZmValue", typeof(double), typeof(Zoomer), new PropertyMetadata(1.0)); public double ZmValue { get => (double)GetValue(ZmValueProperty); set => SetValue(ZmValueProperty, value); }
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

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) { throw new NotImplementedException(string.Format("Under construction: {0}.{1}()", System.Reflection.MethodInfo.GetCurrentMethod()?.DeclaringType?.Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name)); ; }
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
  }
}