using System.Windows;
using System.Windows.Controls;

namespace NavigationDrawerControl
{
  public class HamburgerMenuItem : RadioButton
  {
    static HamburgerMenuItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
  }
}


/// https://youtu.be/mjYXnlufyI8?t=7