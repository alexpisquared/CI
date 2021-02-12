using CsvHelper;
using CsvHelper.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CsvManipulator
{
  public partial class CsvMainWindow : Window
  {
    public CsvMainWindow()
    {
      InitializeComponent();

      var jj = new Converter(@"C:\temp\CI.csv");
      jj.CleanEmptyRowsColumns();
    }

    void onExit(object s, RoutedEventArgs e) { Close(); }

  }
}
