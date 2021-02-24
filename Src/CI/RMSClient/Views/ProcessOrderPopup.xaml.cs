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
using System.Windows.Shapes;

namespace RMSClient.Views
{
  public partial class ProcessOrderPopup : Window
  {
    public ProcessOrderPopup()
    {
      InitializeComponent();
    }

    void Button_Click1(object sender, RoutedEventArgs e)    {      DialogResult = true;      Close();    }
    void Button_Click2(object sender, RoutedEventArgs e)    {      DialogResult = true;      Close();    }
    void Button_Click3(object sender, RoutedEventArgs e)    {      DialogResult = true;      Close();    }
    void Button_Click4(object sender, RoutedEventArgs e)    {      DialogResult = false;      Close(); }
  }
}
