﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NavigationDrawerControl
{
  public class NavDrawer : Control
  {
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(NavDrawer), new PropertyMetadata(false, OnIsOpenPrepertyChanged)); public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(NavDrawer), new PropertyMetadata(null)); public FrameworkElement Content { get => (FrameworkElement)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }
    public static readonly DependencyProperty OpenCloseDurationProperty = DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(NavDrawer), new PropertyMetadata(Duration.Automatic)); public Duration OpenCloseDuration { get => (Duration)GetValue(OpenCloseDurationProperty); set => SetValue(OpenCloseDurationProperty, value); }

    static NavDrawer() => DefaultStyleKeyProperty.OverrideMetadata(typeof(NavDrawer), new FrameworkPropertyMetadata(typeof(NavDrawer)));
    public NavDrawer() => Width = 44;

    static void OnIsOpenPrepertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as NavDrawer)?.OnIsOpenPrepertyChanged();

    private void OnIsOpenPrepertyChanged()
    {
      if (IsOpen)
      {
        OpenMenuAnimated();
      }
      else
      {
        CloseMenuAnimated();
      }
    }

    void OpenMenuAnimated()
    {
      if (Content is null) return;

      Content.Measure(new Size(MaxWidth, MaxHeight));
      BeginAnimation(WidthProperty, new DoubleAnimation(toValue: Content.DesiredSize.Width, OpenCloseDuration));
    }

    void CloseMenuAnimated() => BeginAnimation(WidthProperty, new DoubleAnimation(toValue: 44, OpenCloseDuration));
  }
}
