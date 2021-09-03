using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NavigationDrawerControl
{
  public class HamburgerMenu : Control
  {
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false, OnIsOpenPrepertyChanged)); public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(HamburgerMenu), new PropertyMetadata(null)); public FrameworkElement Content { get => (FrameworkElement)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }
    public static readonly DependencyProperty OpenCloseDurationProperty = DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(HamburgerMenu), new PropertyMetadata(Duration.Automatic)); public Duration OpenCloseDuration { get => (Duration)GetValue(OpenCloseDurationProperty); set => SetValue(OpenCloseDurationProperty, value); }

    static HamburgerMenu() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
    public HamburgerMenu() => Width = 44;

    static void OnIsOpenPrepertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is HamburgerMenu hamburgerMenu)
      {
        hamburgerMenu.OnIsOpenPrepertyChanged();
      }
    }

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

    void CloseMenuAnimated() => BeginAnimation(WidthProperty, new DoubleAnimation(toValue: 40, OpenCloseDuration));
  }
}
